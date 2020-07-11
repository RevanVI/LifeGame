using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

using UnityEngine.UI;

public class DayLightController : MonoBehaviour
{
    public Light2D Light;
    public Gradient LightColor;
    [Tooltip("Day length in seconds")]
    public float DayLength = 86400;
    private float _currentTime;

    //debug 
    [SerializeField]
    private Text _dateText;
    [SerializeField]
    private Image _colorSample;
    //

    void Start()
    {
        
    }

    void Update()
    {
        System.DateTime curTime = System.DateTime.Now;
        _currentTime = (curTime.Hour * 60 + curTime.Minute) * 60 + curTime.Second;
        float value = _currentTime / DayLength;
        Light.color = LightColor.Evaluate(value);

        _dateText.text = curTime.ToLongTimeString();
        _colorSample.color = Light.color;
    }
}
