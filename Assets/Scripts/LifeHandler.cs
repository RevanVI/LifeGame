using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeHandler : MonoBehaviour
{
    private List<LifeDot> _freeLifes;
    private List<LifeDot> _activeLifes;

    public GameObject LifePrefab;

    public void Initialize(int tileCount = 100)
    {
        _activeLifes = new List<LifeDot>();
        _freeLifes = new List<LifeDot>();

        int startCount = tileCount / 2;
        for (int i = 0; i < startCount; ++i)
        {
            GameObject lifeGameObject = Instantiate(LifePrefab, transform);
            LifeDot life = lifeGameObject.GetComponent<LifeDot>();
            lifeGameObject.SetActive(false);
            _freeLifes.Add(life);
        }
    }

    public int GetActiveLifesCount()
    {
        return _activeLifes.Count;
    }

    public void AddNewLife(LifeDot life)
    {
        _activeLifes.Add(life);
    }

    public void ReleaseLife(LifeDot life)
    {
        _activeLifes.Remove(life);
        life.gameObject.SetActive(false);
        life.Age = 0;
        _freeLifes.Add(life);
        
    }

    public LifeDot TakeLife()
    {
        LifeDot requestedLife;
        if (_freeLifes.Count != 0)
        {
            requestedLife = _freeLifes[0];
            _freeLifes.RemoveAt(0);
        }
        else
        {
            GameObject lifeGameObject = Instantiate(LifePrefab, transform);
            requestedLife = gameObject.GetComponent<LifeDot>();
            lifeGameObject.SetActive(false);
        }

        _activeLifes.Add(requestedLife);
        return requestedLife;
    }
}
