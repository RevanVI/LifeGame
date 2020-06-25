using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class ControlPanel : MonoBehaviour
{
    private Toggle _spawnButton;

    public BoolEvent OnSpawnButtonClick;

    private void Awake()
    {
        OnSpawnButtonClick = new BoolEvent();
    }


    // Start is called before the first frame update
    void Start()
    {
        _spawnButton.onValueChanged.AddListener(ProcessSpawnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessSpawnButtonClick(bool status)
    {
        OnSpawnButtonClick.Invoke(status);
    }

    public void ClearToggles()
    {
        _spawnButton.isOn = false;
    }
}
