using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameUI : MonoBehaviour
{
    public Button ModeButton;
    private Text _modeButtonText;
    public Button StepButton;
    private Text _stepButtonText;

    public string AutoModeText = "Auto";
    public string ManualModeText = "Manual";

    public UnityEvent OnModeButtonClick;
    public UnityEvent OnStepButtonClick;

    private void Awake()
    {
        _modeButtonText = ModeButton.gameObject.GetComponentInChildren<Text>();
        _stepButtonText = StepButton.gameObject.GetComponentInChildren<Text>();

        OnModeButtonClick = new UnityEvent();
        OnStepButtonClick = new UnityEvent();

        ModeButton.onClick.AddListener(ProcessModeButtonClick);
        StepButton.onClick.AddListener(ProcessStepButtonClick);
    }

    private void Start()
    {
        ChangeMode();
    }

    public void ChangeMode()
    {
        if (GameController.Instance.Mode == GameController.GameMode.Auto)
        {
            _modeButtonText.text = AutoModeText;
            StepButton.interactable = false;
        }
        else
        {
            _modeButtonText.text = ManualModeText;
            StepButton.interactable = true;
        }
    }

    public void ProcessModeButtonClick()
    {
        OnModeButtonClick.Invoke();
    }

    public void ProcessStepButtonClick()
    {
        OnStepButtonClick.Invoke();
    }
}
