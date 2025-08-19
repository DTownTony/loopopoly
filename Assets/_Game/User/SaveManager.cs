using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveManager
{
    private static readonly string _saveDataPath = $"{Application.persistentDataPath}/";

    public static void Save(GameData data, string saveName = "autosave")
    {
        try
        {
            var saveDataJson = JsonUtility.ToJson(data);
            File.WriteAllText($"{_saveDataPath}{saveName}.save", saveDataJson);
        }
        catch (Exception e)
        {
            Debug.LogError("Unable to save data!\nException: " + e);
        }
    }

    public static GameData Load(string saveName = "autosave")
    {
        GameData data = null;
        var path = $"{_saveDataPath}{saveName}.save";
        try
        {
            data = JsonUtility.FromJson<GameData>(File.ReadAllText(path));
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to read JSON at filePath: " + path + " - " + e.Message);
        }

        return data;
    }

    public static bool HasSave(string saveName = "autosave")
    {
        var path = $"{_saveDataPath}{saveName}.save";
        return File.Exists(path);
    }
    
    public static void DeleteSave(string saveName = "autosave")
    {
        var path = $"{_saveDataPath}{saveName}.save";
        File.Delete(path);
    }
    
    #region Editor Tools

#if UNITY_EDITOR
    [MenuItem("Tools/Save/Open Directory")]
    public static void OpenDirectory()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }

    [MenuItem("Tools/Save/Delete Data")]
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        Directory.Delete(Application.persistentDataPath, true);
    }
#endif

    #endregion
}