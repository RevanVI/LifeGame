using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeDot : MonoBehaviour
{
    public int Age;

    // Start is called before the first frame update
    void Start()
    {
        Age = 0;
        GameController.Instance.RegisterLife(this); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public UnityEvent OnDie = new UnityEvent();
}
