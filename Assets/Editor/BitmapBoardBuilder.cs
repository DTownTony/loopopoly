#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class BitmapBoardBuilder : EditorWindow
{
    [Header("Source")]
    [SerializeField] private Texture2D sourceTexture;

    [Header("Placement")]
    [SerializeField] private float space = 1f;              // distance between nodes
    [SerializeField] private float yLevel = 0f;             // Y height for all nodes
    [SerializeField] private bool centerOnOrigin = true;
    [SerializeField] private bool invertY = true;           // Top row first (image-space) -> world Z

    [Header("Path Options")]
    [SerializeField] private bool use4Connectivity = true;  // 4-neighbors (on-grid). Turn off for 8-neighbors.

    [Header("Node Prefab (optional)")]
    [SerializeField] private GameObject placementPrefab;    // Drag a prefab here to spawn it per node
    [SerializeField] private Vector3 nodeScale = Vector3.one;

    [Header("Output")]
    [SerializeField] private string prefabFolder = "Assets/Boards";
    [SerializeField] private string boardNameOverride = ""; // defaults to Board_<textureName>

    [MenuItem("Tools/Build Board From Bitmap")]
    private static void ShowWindow()
    {
        var win = GetWindow<BitmapBoardBuilder>("Bitmap Board Builder");
        win.minSize = new Vector2(460, 380);
        win.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Source", EditorStyles.boldLabel);
        sourceTexture = (Texture2D)EditorGUILayout.ObjectField("Texture", sourceTexture, typeof(Texture2D), false);

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Placement", EditorStyles.boldLabel);
        space = EditorGUILayout.FloatField("Space", Mathf.Max(0.0001f, space));
        yLevel = EditorGUILayout.FloatField("Y Level", yLevel);
        centerOnOrigin = EditorGUILayout.Toggle("Center On Origin", centerOnOrigin);
        invertY = EditorGUILayout.Toggle("Invert Y (Top Row First)", invertY);

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Path Options", EditorStyles.boldLabel);
        use4Connectivity = EditorGUILayout.Toggle("Use 4-Connectivity", use4Connectivity);

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Node Prefab (optional)", EditorStyles.boldLabel);
        placementPrefab = (GameObject)EditorGUILayout.ObjectField("Placement Prefab", placementPrefab, typeof(GameObject), false);
        nodeScale = EditorGUILayout.Vector3Field("Node Scale", nodeScale);

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Output", EditorStyles.boldLabel);
        prefabFolder = EditorGUILayout.TextField("Prefab Folder", prefabFolder);
        boardNameOverride = EditorGUILayout.TextField("Board Name Override", boardNameOverride);

        EditorGUILayout.Space(12);
        using (new EditorGUI.DisabledScope(sourceTexture == null))
        {
            if (GUILayout.Button("Build Board Prefab", GUILayout.Height(38)))
            {
                BuildBoard();
            }
        }

        EditorGUILayout.HelpBox(
            "Red = start; Black/Blue = path; Blue marks special events; White/transparent ignored. " +
            "Nodes spawn CLOCKWISE for loops and are rotated based on step direction: " +
            "X moves -> yaw 0°/+X or 180°/−X, Y/diagonal -> yaw 90°. " +
            "Specials are sent via Board.SetSpecialEventIndexes(int) per index.",
            MessageType.Info);
    }

    private void BuildBoard()
    {
        if (sourceTexture == null)
        {
            ShowNotification(new GUIContent("No texture selected."));
            return;
        }

        // Ensure texture is readable
        var texPath = AssetDatabase.GetAssetPath(sourceTexture);
        if (string.IsNullOrEmpty(texPath))
        {
            EditorUtility.DisplayDialog("Error", "Texture must be an asset inside the project.", "OK");
            return;
        }

        var ti = AssetImporter.GetAtPath(texPath) as TextureImporter;
        if (ti != null && !ti.isReadable)
        {
            ti.isReadable = true;
            ti.textureCompression = TextureImporterCompression.Uncompressed;
            try { ti.SaveAndReimport(); }
            catch
            {
                EditorUtility.DisplayDialog("Error", "Could not set texture as readable. Check import settings.", "OK");
                return;
            }
        }

        int w = sourceTexture.width;
        int h = sourceTexture.height;
        if (w <= 0 || h <= 0)
        {
            EditorUtility.DisplayDialog("Error", "Texture has invalid size.", "OK");
            return;
        }

        var pixels = sourceTexture.GetPixels32();
        var boardMask = new bool[w * h];
        var redMask = new bool[w * h];
        var blueMask = new bool[w * h]; // track blue pixels for specials
        int boardCount = 0;

        // Build masks: board = red or black or blue; white & transparent ignored
        for (int i = 0; i < pixels.Length; i++)
        {
            var c = pixels[i];
            bool isTransparent = c.a <= 10;

            bool isWhite = (c.r > 230 && c.g > 230 && c.b > 230) && !isTransparent;
            bool isBlack = (c.r < 30 && c.g < 30 && c.b < 30) && !isTransparent;
            bool isRed   = (c.r > 200 && c.g < 40  && c.b < 40 ) && !isTransparent;
            bool isBlue  = (c.b > 200 && c.r < 40  && c.g < 40 ) && !isTransparent;

            if (!isWhite && (isBlack || isRed || isBlue))
            {
                boardMask[i] = true;
                boardCount++;
                if (isRed)  redMask[i]  = true;
                if (isBlue) blueMask[i] = true;
            }
        }

        if (boardCount == 0)
        {
            EditorUtility.DisplayDialog("Error", "No red/black/blue path pixels found (white/transparent are ignored).", "OK");
            return;
        }

        // Find start = first red pixel
        Vector2Int start = new Vector2Int(-1, -1);
        for (int i = 0; i < w * h; i++)
        {
            if (redMask[i])
            {
                start = new Vector2Int(i % w, i / w);
                break;
            }
        }
        if (start.x < 0)
        {
            EditorUtility.DisplayDialog("Error", "No red start pixel found.", "OK");
            return;
        }

        // Walk the path starting at red
        var path = TracePath(start, boardMask, w, h, use4Connectivity);
        if (path.Count == 0)
        {
            EditorUtility.DisplayDialog("Error", "Failed to trace a path from the red pixel.", "OK");
            return;
        }

        // If looped, ensure CLOCKWISE order (shoelace area > 0 => CCW, so flip when > 0)
        bool isLoop = path.Count > 2 && path[0] == path[path.Count - 1];
        if (isLoop)
        {
            path.RemoveAt(path.Count - 1); // remove duplicate last
            float area = PolygonAreaForCCW(path, w, h, invertY);
            if (area > 0f) // flip to make CW
            {
                path.Reverse();
                // rotate so that 'start' is at index 0 again
                int idx = IndexOfCoord(path, start);
                if (idx > 0)
                {
                    var rotated = new List<Vector2Int>(path.Count);
                    for (int i = 0; i < path.Count; i++)
                        rotated.Add(path[(i + idx) % path.Count]);
                    path = rotated;
                }
            }
        }

        // Create root & children as Pos (i) — using prefab if provided
        string boardName = string.IsNullOrEmpty(boardNameOverride) ? $"Board_{sourceTexture.name}" : boardNameOverride;
        var root = new GameObject(boardName);
        Undo.RegisterCreatedObjectUndo(root, "Create Board Root");

        float xOffset = 0f, zOffset = 0f;
        if (centerOnOrigin)
        {
            // center using full image bounds for consistent spacing from edges
            xOffset = -((w - 1) * 0.5f * space);
            zOffset = -((h - 1) * 0.5f * space);
        }

        var nodes = new Transform[path.Count];

        for (int i = 0; i < path.Count; i++)
        {
            var p = path[i];
            int row = invertY ? (h - 1 - p.y) : p.y;

            float x = p.x * space + xOffset;
            float y = yLevel;
            float z = row * space + zOffset;

            GameObject childGo;
            if (placementPrefab != null)
            {
                var assetType = PrefabUtility.GetPrefabAssetType(placementPrefab);
                if (assetType != PrefabAssetType.NotAPrefab)
                {
                    var inst = PrefabUtility.InstantiatePrefab(placementPrefab, root.transform) as GameObject;
                    childGo = inst != null ? inst : new GameObject();
                }
                else
                {
                    childGo = (GameObject)PrefabUtility.InstantiatePrefab(placementPrefab, root.transform);
                    if (childGo == null)
                    {
                        childGo = UnityEngine.Object.Instantiate(placementPrefab);
                        childGo.transform.SetParent(root.transform, false);
                    }
                }
            }
            else
            {
                childGo = new GameObject();
                childGo.transform.SetParent(root.transform, false);
            }

            childGo.name = $"Pos ({i})";
            Undo.RegisterCreatedObjectUndo(childGo, "Create Node");
            var t = childGo.transform;
            t.localPosition = new Vector3(x, y, z);
            t.localScale = nodeScale;

            // --- Direction-based rotation (force 0 / 180 / 90; never -180) ---
            Vector2Int refPt;
            if (i > 0) refPt = path[i - 1];          // compare to previous
            else if (path.Count > 1) refPt = path[1]; // or look ahead for first node
            else refPt = p;

            int dx = p.x - refPt.x;   // image-space
            int dy = p.y - refPt.y;

            float yaw;
            if (dy == 0 && dx != 0)
            {
                // X move: +X -> 0°, -X -> 180°
                yaw = dx > 0 ? 0f : 180f;
            }
            else
            {
                // Y (or diagonal fallback) -> 90°
                yaw = 90f;
            }
            t.localEulerAngles = new Vector3(0f, NormalizeYaw360(yaw), 0f);

            nodes[i] = t;
        }

        // --- Attach Board.cs on root and auto-wire common fields ---
        var boardType = FindTypeByName("Board");
        MonoBehaviour board = null;
        if (boardType != null && typeof(MonoBehaviour).IsAssignableFrom(boardType))
        {
            board = root.AddComponent(boardType) as MonoBehaviour;

            // Try common names (adds support for fields like "_boardPositions", "positions", etc.)
            TryAssignTransforms(board, "_boardPositions", nodes);
            TryAssignTransforms(board, "boardPositions",  nodes);
            TryAssignTransforms(board, "positions",       nodes);
            TryAssignTransforms(board, "nodes",           nodes);
            TryAssignTransforms(board, "path",            nodes);
            TryAssignTransforms(board, "waypoints",       nodes);

            TryAssignFloat(board, "space",   space);
            TryAssignFloat(board, "spacing", space);
            TryAssignFloat(board, "yLevel",  yLevel);

            EditorUtility.SetDirty(board);
        }
        else
        {
            Debug.LogWarning("Board.cs not found (class named 'Board'). Component not added.");
        }

        // --- Collect specials from BLUE pixels and send via SetSpecialEventIndexes(int) ---
        if (board != null)
        {
            // dedupe specials so we don't spam duplicates
            var specials = new HashSet<int>();
            for (int i = 0; i < path.Count; i++)
            {
                var p = path[i];
                int idxPix = p.y * w + p.x; // image-space index
                if (idxPix >= 0 && idxPix < blueMask.Length && blueMask[idxPix])
                {
                    specials.Add(i);
                }
            }

            // Try to clear existing specials first (if your Board has such a method or field)
            TryClearExistingSpecials(board);

            // Add each special via SetSpecialEventIndexes(int)
            foreach (var idx in specials)
                TryInvokeAddSpecialIndex(board, idx);
        }

        // Save as prefab
        if (!AssetDatabase.IsValidFolder(prefabFolder))
        {
            CreateFolderRecursive(prefabFolder);
        }
        string prefabPath = Path.Combine(prefabFolder, boardName + ".prefab").Replace("\\", "/");
        var saved = PrefabUtility.SaveAsPrefabAsset(root, prefabPath, out bool success);

        if (success && saved != null)
        {
            EditorGUIUtility.PingObject(saved);
            Debug.Log($"Board prefab created at: {prefabPath}  |  Nodes: {nodes.Length}");
        }
        else
        {
            EditorUtility.DisplayDialog("Save Failed", "Could not save prefab. Check folder path and permissions.", "OK");
        }

        DestroyImmediate(root);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private List<Vector2Int> TracePath(Vector2Int start, bool[] boardMask, int w, int h, bool fourConn)
    {
        var path = new List<Vector2Int>(1024);
        var cur = start;
        var prev = new Vector2Int(int.MinValue, int.MinValue);
        path.Add(cur);

        // 4-neighbor order: Right, Down, Left, Up (image-space y grows downward)
        var n4 = new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1) };
        // 8-neighbor (diagonals after cardinals)
        var n8 = new Vector2Int[]
        {
            new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(-1,0), new Vector2Int(0,-1),
            new Vector2Int(1,1), new Vector2Int(-1,1), new Vector2Int(-1,-1), new Vector2Int(1,-1)
        };

        int maxSteps = w * h * 2; // guard
        int steps = 0;

        int dirIndex = -1; // 0 R,1 D,2 L,3 U

        while (steps++ < maxSteps)
        {
            if (dirIndex >= 0)
            {
                Vector2Int next = new Vector2Int(int.MinValue, int.MinValue);
                int[] tryOrder = new int[] { (dirIndex + 3) & 3, dirIndex, (dirIndex + 1) & 3, (dirIndex + 2) & 3 };
                for (int k = 0; k < 4; k++)
                {
                    var d = n4[tryOrder[k]];
                    var cand = new Vector2Int(cur.x + d.x, cur.y + d.y);
                    if (cand == prev) continue;
                    if (InBounds(cand, w, h) && boardMask[cand.y * w + cand.x])
                    {
                        next = cand;
                        dirIndex = tryOrder[k];
                        break;
                    }
                }

                if (next.x == int.MinValue && !fourConn)
                {
                    for (int k = 4; k < 8; k++)
                    {
                        var d = n8[k];
                        var cand = new Vector2Int(cur.x + d.x, cur.y + d.y);
                        if (cand == prev) continue;
                        if (InBounds(cand, w, h) && boardMask[cand.y * w + cand.x])
                        {
                            next = cand;
                            break;
                        }
                    }
                }

                if (next.x == int.MinValue) break;

                prev = cur;
                cur = next;
                path.Add(cur);

                if (cur == start && path.Count > 2)
                {
                    path.Add(start); // mark loop
                    break;
                }
            }
            else
            {
                // first step
                Vector2Int next = new Vector2Int(int.MinValue, int.MinValue);
                int firstDir = -1;

                for (int k = 0; k < 4; k++)
                {
                    var d = n4[k];
                    var cand = new Vector2Int(cur.x + d.x, cur.y + d.y);
                    if (InBounds(cand, w, h) && boardMask[cand.y * w + cand.x])
                    {
                        next = cand;
                        firstDir = k;
                        break;
                    }
                }
                if (next.x == int.MinValue && !fourConn)
                {
                    for (int k = 4; k < 8; k++)
                    {
                        var d = n8[k];
                        var cand = new Vector2Int(cur.x + d.x, cur.y + d.y);
                        if (InBounds(cand, w, h) && boardMask[cand.y * w + cand.x])
                        {
                            next = cand;
                            firstDir = NearestCardinalIndex(d);
                            break;
                        }
                    }
                }

                if (next.x == int.MinValue) break;

                dirIndex = firstDir < 0 ? 0 : firstDir;
                prev = cur;
                cur = next;
                path.Add(cur);
            }
        }

        return path;
    }

    private static bool InBounds(Vector2Int p, int w, int h)
    {
        return p.x >= 0 && p.y >= 0 && p.x < w && p.y < h;
    }

    private static int NearestCardinalIndex(Vector2Int d)
    {
        if (Mathf.Abs(d.x) >= Mathf.Abs(d.y)) return d.x >= 0 ? 0 : 2; // Right/Left
        return d.y >= 0 ? 1 : 3; // Down/Up (image-space)
    }

    private static int IndexOfCoord(List<Vector2Int> list, Vector2Int v)
    {
        for (int i = 0; i < list.Count; i++) if (list[i] == v) return i;
        return -1;
    }

    // Shoelace area on pixel coords mapped to world-plane indices (x, row)
    private static float PolygonAreaForCCW(List<Vector2Int> pts, int w, int h, bool invertY)
    {
        if (pts.Count < 3) return 0f;
        float area = 0f;
        int n = pts.Count;
        for (int i = 0; i < n; i++)
        {
            var a = pts[i];
            var b = pts[(i + 1) % n];
            float ax = a.x;
            float ay = invertY ? (h - 1 - a.y) : a.y;
            float bx = b.x;
            float by = invertY ? (h - 1 - b.y) : b.y;
            area += (ax * by - bx * ay);
        }
        return area * 0.5f;
    }

    private static float NormalizeYaw360(float yaw)
    {
        yaw %= 360f;
        if (yaw < 0f) yaw += 360f;
        return yaw;
    }

    private void CreateFolderRecursive(string fullPath)
    {
        fullPath = fullPath.Replace("\\", "/"); // normalize
        string[] parts = fullPath.Split('/');
        if (parts.Length <= 1 || parts[0] != "Assets") return;

        string cur = "Assets";
        for (int i = 1; i < parts.Length; i++)
        {
            string next = cur + "/" + parts[i];
            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(cur, parts[i]);
            }
            cur = next;
        }
    }

    // ------- Board auto-wiring helpers -------
    private static Type FindTypeByName(string simpleName)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < assemblies.Length; i++)
        {
            var types = assemblies[i].GetTypes();
            for (int j = 0; j < types.Length; j++)
            {
                var t = types[j];
                if (t.Name == simpleName) return t;
            }
        }
        return null;
    }

    private static void TryAssignTransforms(object target, string memberName, Transform[] values)
    {
        var t = target.GetType();

        var f = t.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (f != null)
        {
            var ft = f.FieldType;
            if (ft.IsArray && ft.GetElementType() == typeof(Transform))
            {
                f.SetValue(target, values);
                return;
            }
            if (typeof(IList).IsAssignableFrom(ft))
            {
                var genArgs = ft.IsGenericType ? ft.GetGenericArguments() : Type.EmptyTypes;
                if (genArgs.Length == 1 && genArgs[0] == typeof(Transform))
                {
                    var list = Activator.CreateInstance(ft) as IList;
                    for (int i = 0; i < values.Length; i++) list!.Add(values[i]);
                    f.SetValue(target, list);
                    return;
                }
            }
        }

        var p = t.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (p != null && p.CanWrite)
        {
            var pt = p.PropertyType;
            if (pt.IsArray && pt.GetElementType() == typeof(Transform))
            {
                p.SetValue(target, values, null);
                return;
            }
            if (typeof(IList).IsAssignableFrom(pt))
            {
                var genArgs = pt.IsGenericType ? pt.GetGenericArguments() : Type.EmptyTypes;
                if (genArgs.Length == 1 && genArgs[0] == typeof(Transform))
                {
                    var list = Activator.CreateInstance(pt) as IList;
                    for (int i = 0; i < values.Length; i++) list!.Add(values[i]);
                    p.SetValue(target, list, null);
                    return;
                }
            }
        }
    }

    private static void TryAssignFloat(object target, string memberName, float value)
    {
        var t = target.GetType();

        var f = t.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (f != null && (f.FieldType == typeof(float) || f.FieldType == typeof(double)))
        {
            if (f.FieldType == typeof(float)) f.SetValue(target, value);
            else f.SetValue(target, (double)value);
            return;
        }

        var p = t.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (p != null && p.CanWrite && (p.PropertyType == typeof(float) || p.PropertyType == typeof(double)))
        {
            if (p.PropertyType == typeof(float)) p.SetValue(target, value, null);
            else p.SetValue(target, (double)value, null);
        }
    }

    // ---- Specials plumbing for your new API ----
    private static void TryClearExistingSpecials(object board)
    {
        var t = board.GetType();

        // Prefer a dedicated clear method if it exists
        var clearMethod = t.GetMethod("ClearSpecialEventIndexes", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (clearMethod != null && clearMethod.GetParameters().Length == 0)
        {
            clearMethod.Invoke(board, null);
            return;
        }

        // Fallback: clear a serialized List<int> field named _specialEventIndexes if present
        var field = t.GetField("_specialEventIndexes", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (field != null)
        {
            var val = field.GetValue(board) as IList;
            if (val != null) val.Clear();
        }
    }

    private static void TryInvokeAddSpecialIndex(object board, int index)
    {
        var t = board.GetType();
        var m = t.GetMethod("SetSpecialEventIndexes", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (m == null) return;

        var parms = m.GetParameters();
        if (parms.Length != 1) return;

        // If param is int or convertible, pass the index
        var pt = parms[0].ParameterType;
        object arg = index;

        if (pt != typeof(int))
        {
            try
            {
                arg = Convert.ChangeType(index, pt);
            }
            catch
            {
                return; // incompatible signature
            }
        }

        m.Invoke(board, new object[] { arg });
    }
}
#endif
