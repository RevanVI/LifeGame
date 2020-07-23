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

    public void ChangeMode()
    {
        if (GameController.Instance.GameMode == GameController.EGameMode.Auto)
        {
            StepButton.interactable = false;
        }
        else
        {
            StepButton.interactable = true;
        }
    }

    public void ProcessModeButtonClick(bool isOn)
    {
        OnModeToggleClick.Invoke(isOn);
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
