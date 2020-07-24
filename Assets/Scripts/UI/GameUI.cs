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
    private bool _modeChangeByToggle = true;

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
        ChangeMode();
    }

    public void ChangeMode(bool changedByToggle = true)
    {
        Debug.Log($"ChangeMode {changedByToggle}");
        _modeChangeByToggle = changedByToggle;
        bool status = (GameController.Instance.GameMode == GameController.EGameMode.Auto) ? false : true;
        StepButton.interactable = status;
        if (!_modeChangeByToggle)
            ModeToggle.isOn = status;
    }

    public void ProcessModeButtonClick(bool isOn)
    {
        Debug.Log($"ProcessModeButtonClick _modeChangeByToggle {_modeChangeByToggle}, isOn {isOn}");
        if (_modeChangeByToggle)
            OnModeToggleClick.Invoke(isOn);
        else
            _modeChangeByToggle = true;
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
