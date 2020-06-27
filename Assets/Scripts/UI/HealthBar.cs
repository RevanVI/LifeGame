using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    public void SetMaxValue(float maxValue)
    {
        _slider.maxValue = maxValue;
    }

    public void SetValue(float value)
    {
        if (value < 0)
            _slider.value = 0;
        else if (value > _slider.maxValue)
            _slider.value = _slider.maxValue;
        else
            _slider.value = value;
    }

    public void SetMode(bool isIntValues)
    {
        _slider.wholeNumbers = isIntValues;
    }

    public float GetValue()
    {
        return _slider.value;
    }

    void Start()
    {
        
    }
}
