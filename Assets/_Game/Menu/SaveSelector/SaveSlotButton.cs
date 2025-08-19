using UnityEngine;
using UnityEngine.UI;

public class SaveSlotButton : MonoBehaviour
{
    [SerializeField] private string _key;
    [SerializeField] private GameObject _newGameContainer;
    [SerializeField] private GameObject _loadGameContainer;
    [SerializeField] private Button _button;
    [SerializeField] private Button _deleteButton;
    
    [SerializeField] private GameSetup _gameSetup;

    private void Awake()
    {
        _deleteButton.onClick.AddListener(DeleteButtonPressed);
        Refresh();
    }
    
    private void Refresh()
    {
        _button.onClick.RemoveAllListeners();
        
        var hasSave = SaveManager.HasSave(_key);
        if (hasSave)
        {
            _newGameContainer.SetActive(false);
            _loadGameContainer.SetActive(true);
 
            _button.onClick.AddListener(LoadGame);
        }
        else
        {
            _newGameContainer.SetActive(true);
            _loadGameContainer.SetActive(false);
            
            _button.onClick.AddListener(NewGame);
        }
    }

    private void NewGame()
    {
        var newData = new GameData();
        _gameSetup.LoadGame(newData);
        SaveManager.Save(newData, _key);
    }

    private void LoadGame()
    {
        var gameDataSave = SaveManager.Load(_key);
        _gameSetup.LoadGame(gameDataSave);
    }
    
    private void DeleteButtonPressed()
    {
        SaveManager.DeleteSave(_key);
        Refresh();
    }
}