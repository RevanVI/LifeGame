using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class ControlPanel : MonoBehaviour
{
    [SerializeField]
    private Toggle _spawnButton;
    [SerializeField]
    private Toggle _removeButton;
    public BoolEvent OnSpawnButtonClick;
    public BoolEvent OnRemoveButtonClick;

    private int _activeButton;

    private void Awake()
    {
        OnSpawnButtonClick = new BoolEvent();
        OnRemoveButtonClick = new BoolEvent();
    }


    // Start is called before the first frame update
    void Start()
    {
        _spawnButton.onValueChanged.AddListener(ProcessSpawnButtonClick);
        _removeButton.onValueChanged.AddListener(ProcessDebugButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessSpawnButtonClick(bool status)
    {
        OnSpawnButtonClick.Invoke(status);
    }

    public void ProcessDebugButtonClick(bool status)
    {
        OnRemoveButtonClick.Invoke(status);
    }

    public void ClearToggles()
    {
        _spawnButton.isOn = false;
        _removeButton.isOn = false;
    }
}
