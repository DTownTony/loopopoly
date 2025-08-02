using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private Image _fill;
    
    public void Show()
    {
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