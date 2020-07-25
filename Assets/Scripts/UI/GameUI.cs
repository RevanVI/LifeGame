using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameUI : MonoBehaviour
{
    public Toggle ModeToggle;
    public Button StepButton;
    private Text _stepButtonText;

    [SerializeField]
    private Text _yearsText;


    public string AutoModeText = "Auto";
    public string ManualModeText = "Manual";

    public BoolEvent OnModeToggleClick;
    public UnityEvent OnStepButtonClick;

    //variable for control toggle state changes by click or programmatically and prevent OnModeToggleClick event trigger
    private bool _ignoreNextToggleValueChange = false;
    private GameController.EGameMode _curVisibleMode = GameController.EGameMode.Auto;

    private void Awake()
    {
        _stepButtonText = StepButton.gameObject.GetComponentInChildren<Text>();

        OnModeToggleClick = new BoolEvent();
        OnStepButtonClick = new UnityEvent();

        ModeToggle.onValueChanged.AddListener(ProcessModeButtonClick);
        StepButton.onClick.AddListener(ProcessStepButtonClick);
    }

    private void Start()
    {
        UpdateMode();
    }

    public void UpdateMode(bool changedProgrammatically = false)
    {
        bool status = (GameController.Instance.GameMode == GameController.EGameMode.Auto) ? false : true;
        if (_curVisibleMode != GameController.Instance.GameMode)
        {
            StepButton.interactable = status;
            _ignoreNextToggleValueChange = changedProgrammatically;
            if (changedProgrammatically)
                ModeToggle.isOn = status;
            _curVisibleMode = GameController.Instance.GameMode;
        }
        
    }

    public void ProcessModeButtonClick(bool isOn)
    {
        if (!_ignoreNextToggleValueChange)
            OnModeToggleClick.Invoke(isOn);
        else
            _ignoreNextToggleValueChange = false;
    }

    public void ProcessStepButtonClick()
    {
        OnStepButtonClick.Invoke();
    }

    public void UpdateYearsText(int years)
    {
        _yearsText.text = years.ToString();
    }

}
