using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    private GameData _currentGameData;
    
    public void LoadGame(GameData data)
    {
        _currentGameData = data;
        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.LoadScene("Game");
    }

    private void SceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        GameController.Instance.Setup(_currentGameData);
        _currentGameData = null;
    }
}