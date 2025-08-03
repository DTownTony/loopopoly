using DG.Tweening;
using TMPro;
using UnityEngine;

public class EventDetailDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private TMP_Text _detailText;
    [SerializeField] private Camera _camera;

    private Vector2 _startingPosition;

    private Sequence _showSequence;
    
    private void Awake()
    {
        _startingPosition = _rect.anchoredPosition;
    }

    public void ShowMessage(string message, Transform target = null, Color32 col = default)
    {
        _rect.anchoredPosition = _startingPosition;
        _detailText.text = message;
        _detailText.color = col;
        _detailText.alpha = 1;
        
        gameObject.SetActive(true);
        
        if (target != null)
            PositionText(target);
        
        _showSequence?.Kill();
        _showSequence = DOTween.Sequence();
        _showSequence.Append(_rect.DOMoveY(_rect.position.y + 100, 1.5f));
        _showSequence.Join(_detailText.DOFade(0, .1f).SetDelay(1.4f));
    }

    private void PositionText(Transform target)
    {
        var position = target.transform.position;
        position.y += 1.25f;

        var screenPosition = _camera.WorldToScreenPoint(position);
        _rect.position = screenPosition;
    }
}
