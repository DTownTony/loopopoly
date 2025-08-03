using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Image _fill;
    
    public void Show(Transform target, float yOffset)
    {
        var targetPosition = target.position;
        targetPosition.y += yOffset;
        var position = _camera.WorldToScreenPoint(targetPosition);
        position.x = transform.position.x; //keep in center
        
        transform.position = position;
        
        gameObject.SetActive(true);
        
        SetFill(1);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetFill(float percent)
    {
        _fill.fillAmount = percent;
    }
}