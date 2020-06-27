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
    private Toggle _debugButton;
    public BoolEvent OnSpawnButtonClick;
    public BoolEvent OnDebugButtonClick;

    private int _activeButton;

    private void Awake()
    {
        OnSpawnButtonClick = new BoolEvent();
        OnDebugButtonClick = new BoolEvent();
    }


    // Start is called before the first frame update
    void Start()
    {
        _spawnButton.onValueChanged.AddListener(ProcessSpawnButtonClick);
        _debugButton.onValueChanged.AddListener(ProcessDebugButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessSpawnButtonClick(bool status)
    {
        Debug.Log("SpawnBUttonClick");
        OnSpawnButtonClick.Invoke(status);
    }

    public void ProcessDebugButtonClick(bool status)
    {
        Debug.Log("DebugBUttonClick");
        OnDebugButtonClick.Invoke(status);
    }

    public void ClearToggles()
    {
        Debug.Log("ClearToggles");
        _spawnButton.isOn = false;
        _debugButton.isOn = false;
    }
}
