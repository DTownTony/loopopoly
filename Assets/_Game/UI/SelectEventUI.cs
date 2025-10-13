using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SelectEventUI : EventUIBase<SelectEventUIArgs>
{
    [SerializeField] private SelectEventChoiceUI _choiceUIPrefab;
    [SerializeField] private Transform _choiceContainer;
    
    public override void Show(SelectEventUIArgs args)
    {
        _currentArgs = args;
        
        gameObject.SetActive(true);

        foreach (var choice in _currentArgs.Choices)
        {
            var choiceUI = Instantiate(_choiceUIPrefab, _choiceContainer);
            choiceUI.Button.onClick.AddListener(choice.OnSelect);
            choiceUI.Button.onClick.AddListener(Hide);
            
            //randomize
            choiceUI.transform.SetSiblingIndex(Random.Range(0, _choiceContainer.childCount));
        }
    }

    protected override void Hide()
    {
        base.Hide();

        //todo: pool
        for (var i = 0; i < _choiceContainer.childCount; i++)
            Destroy(_choiceContainer.GetChild(i).gameObject);
    }
}

public class SelectEventUIArgs : EventUIArgsBase
{
    public SelectEventChoice[] Choices;
    
    public SelectEventUIArgs(string header, Sprite icon, string description, SelectEventChoice[] choices) : base(header, icon, description)
    {
        Choices = choices;
    }
}

public struct SelectEventChoice
{
    public Sprite Icon;
    public UnityAction OnSelect;
}
