using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    public bool Used = false;
    private Rigidbody2D _rgbd2d;

    private void Awake()
    {
        _rgbd2d = GetComponent<Rigidbody2D>();
    }

    public void Enable()
    {
        _rgbd2d.simulated = true;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        _rgbd2d.simulated = false;
        gameObject.SetActive(false);
    }
}
