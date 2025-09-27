using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    private GameData _currentGameData;

    public void SetCurrentGameData(GameData gameData)
    {
        _currentGameData = gameData;
    }
    
    public void LoadGame(GameData data)
    {
        SetCurrentGameData(data);
        //todo: check save if game in progress
        SceneManager.LoadScene("Home");
    }

    public void LoadGameScene()
    {
        SceneManager.sceneLoaded += GameSceneLoaded;
        SceneManager.LoadScene("Game");
    }

    private void GameSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        SceneManager.sceneLoaded -= GameSceneLoaded;
        GameController.Instance.Setup(_currentGameData);
        SetCurrentGameData(null);
    }
}