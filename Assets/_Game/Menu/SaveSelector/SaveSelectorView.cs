using UnityEngine;

public class SaveSelectorView : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    public void Show()
    {
        _canvas.enabled = true;
    }
}
