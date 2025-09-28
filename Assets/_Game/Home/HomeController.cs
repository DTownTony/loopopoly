using UnityEngine;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    [SerializeField] private TokenUnlockHandler _tokenUnlockHandler;

    private void Start()
    {
        _startButton.onClick.AddListener(StartButtonPressed);

        if (GlobalManagers.Instance.GameProfile.GameData.TokensToUnlock > 0)
            _tokenUnlockHandler.UnlockTokens
                (GlobalManagers.Instance.GameProfile.GameData.TokensToUnlock);
    }

    private void StartButtonPressed()
    {
        GlobalManagers.Instance.GameProfile.LoadGameScene();
    }
}