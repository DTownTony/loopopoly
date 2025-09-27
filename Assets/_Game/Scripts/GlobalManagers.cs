using UnityEngine;

public class GlobalManagers : MonoBehaviour
{
    public static GlobalManagers Instance { get; private set; }

#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        if (Instance == null && !UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Bootstrap"))
        {
            var prefab = Resources.Load<GlobalManagers>("_GlobalManagers");
            Instance = Instantiate(prefab);
            Instance.name = "_GlobalManagers";

            //load slot 1
            //todo: check current scene
            var gameData = SaveManager.HasSave("s1") ? SaveManager.Load("s1") : new GameData();
            Instance.GameSetup.SetCurrentGameData(gameData);
        }
    }
#endif

    public GameSetup GameSetup;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}