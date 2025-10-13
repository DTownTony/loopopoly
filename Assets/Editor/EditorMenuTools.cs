using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EditorMenuTools : MonoBehaviour
{
    #region Scene Methods
    
    [MenuItem("Tools/Scenes/Bootstrap")]
    private static void LoadBootstrapScene()
    {
        LoadMap("Assets/_Game/Scenes/Bootstrap.unity");
    }

    [MenuItem("Tools/Scenes/Menu")]
    private static void LoadMenuScene()
    {
        LoadMap("Assets/_Game/Scenes/Menu.unity");
    }
    
    [MenuItem("Tools/Scenes/Home")]
    private static void LoadHomeScene()
    {
        LoadMap("Assets/_Game/Scenes/Home.unity");
    }
    
    [MenuItem("Tools/Scenes/Game")]
    private static void LoadSmallCaveScene()
    {
        LoadMap("Assets/_Game/Scenes/Game.unity");
    }

    private static void LoadMap(string mapPath)
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(mapPath);
    }
    
    
    #endregion
}