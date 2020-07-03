using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Toggle))]
public class IngameMenu : MonoBehaviour
{
    private Animator _animator;
    private Toggle _toggle;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
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
}
