using UnityEngine;

public class GlobalManagers : MonoBehaviour
{
    private static GlobalManagers _instance;

#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        if (_instance == null && !UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Bootstrap"))
        {
            var prefab = Resources.Load<GlobalManagers>("_GlobalManagers");
            _instance = Instantiate(prefab);
            _instance.name = "_GlobalManagers";

            //var gameData = SaveManager.HasSave() ? SaveManager.GetLatestSave() : new GameData();
            //_instance.GetComponent<GameSetup>().DebugLoadGame(gameData);
        }
    }
#endif
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}