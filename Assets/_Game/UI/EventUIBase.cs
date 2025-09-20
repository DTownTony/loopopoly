using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class EventUIBase<T> : MonoBehaviour where T : EventUIArgsBase
{
    [SerializeField] private EventView _eventView;
    
    [SerializeField] private TMP_Text _headerText;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _descriptionText;

    protected T _currentArgs;

    public virtual void Show(T args)
    {
        _currentArgs = args;
        gameObject.SetActive(true);
        
        _headerText.SetText(_currentArgs.Header);
        _icon.sprite = _currentArgs.Icon;
        _descriptionText.SetText(_currentArgs.Description);
    }
    
    protected virtual void Hide()
    {
        gameObject.SetActive(false);
        _eventView.Hide();
        GameController.Instance.ChangeCurrentState(GameState.WaitingForPlayer);
    }
}

public class EventUIArgsBase
{
    public string Header;
    public Sprite Icon;
    public string Description;

    protected EventUIArgsBase(string header, Sprite icon, string description)
    {
        Header = header;
        Icon = icon;
        Description = description;
    }
}