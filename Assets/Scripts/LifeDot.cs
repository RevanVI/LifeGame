using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeDot : MonoBehaviour
{
    [SerializeField]
    private int[] _ageStages = new int[2];

    public int Age;

    private Animator _animController;


    // Start is called before the first frame update
    void Start()
    {
        Age = 0;
        GameController.Instance.RegisterLife(this);
        _animController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        //anim
        LifeDot.OnDie.Invoke();
    }

    public void Grow()
    {
        ++Age;
        _animController.SetInteger("Age", Age);
    }

    static public UnityEvent OnDie = new UnityEvent();
}
