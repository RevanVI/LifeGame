using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class IngameMenu : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    private Toggle _toggle;

    private int _currentActiveButon = -1;

    [SerializeField]
    private GameObject _settingsMenu;

    // Start is called before the first frame update
    void Start()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(HandleButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleButtonClick(bool isOn)
    {
        if (isOn)
            _animator.SetTrigger("Enable");
        else
            _animator.SetTrigger("Disable");
    }
    
    public void HandleMenuButtonClick(int buttonIndex)
    {
        int oldTab = _currentActiveButon;
        if (_currentActiveButon != -1)
        {
            CloseTab(_currentActiveButon);
            if (buttonIndex == oldTab)
                return;
        }
        ShowTab(buttonIndex);
    }

    private void ShowTab(int index)
    {
        GameController.Instance.Paused = true;
        if (index == 0)
        {
            _settingsMenu.SetActive(true);
        }
        else
        {
            Achievements.AchievementController.Instance.gameObject.SetActive(true);
            Achievements.AchievementController.Instance.UpdateAchievements();
        }
        _currentActiveButon = index;
        InputController.Instance.OnEsc.AddListener(EscHandler);
    }

    private void CloseTab(int index)
    {
        if (index == 0)
        {
            _settingsMenu.SetActive(false);
        }
        else
        {
            Achievements.AchievementController.Instance.gameObject.SetActive(false);
        }
        _currentActiveButon = -1;
        GameController.Instance.Paused = false;
    }

    private void EscHandler(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Escape)
            CloseTab(_currentActiveButon);
        InputController.Instance.OnEsc.RemoveListener(EscHandler);
    }
}
