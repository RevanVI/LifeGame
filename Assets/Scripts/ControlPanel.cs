using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class ControlPanel : MonoBehaviour
{
    [SerializeField]
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
        //_spawnButton.
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

    public void ClearToggles()
    {
        _spawnButton.isOn = false;
    }
}
