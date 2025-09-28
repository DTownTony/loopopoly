using TMPro;
using UnityEngine;

public class TokenUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _amountText;
    
    private void Start()
    {
        GameController.Instance.OnTokenUpdated += TokenUpdated;
        TokenUpdated(GlobalManagers.Instance.GameProfile.GameData.TokensToUnlock);
    }

    private void TokenUpdated(int total)
    {
        _amountText.SetText(total.ToString());
    }
}
