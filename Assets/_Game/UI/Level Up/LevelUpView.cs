using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpView : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private Button _closeButton;

    private void Awake()
    {
        _closeButton.onClick.AddListener(Hide);
    }

    public void Show()
    {
        _canvas.enabled = true;
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, .35f).SetDelay(.5f);

        _closeButton.interactable = false;
        
        //todo: check skill points and enable close button
    }

    private void Hide()
    {
        _canvasGroup.enabled = false;
    }
}
