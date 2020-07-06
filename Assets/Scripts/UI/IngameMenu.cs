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

    public void HandleAchievementButtonClick()
    {
        bool isActive = Achievements.AchievementController.Instance.gameObject.activeSelf;
        //_animator.SetBool("AchievementMenuActive", !currentState);
        //if (!currentState)
        if (!isActive)
        {
            Achievements.AchievementController.Instance.gameObject.SetActive(true);
            Achievements.AchievementController.Instance.UpdateAchievements();
        }
        else
        {
            Achievements.AchievementController.Instance.gameObject.SetActive(false);
        }
    }
}
