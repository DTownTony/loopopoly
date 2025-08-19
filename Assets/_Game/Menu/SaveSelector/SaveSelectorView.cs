using UnityEngine;
using UnityEngine.UI;

public class SaveSelectorView : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _backButton;

    private void Awake()
    {
        _backButton.onClick.AddListener(Hide);
    }

    public void Show()
    {
        _canvas.enabled = true;
    }

    private void Hide()
    {
        _canvas.enabled = false;
    }
}
