using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PowerBar : MonoBehaviour
{
    private BoxCollider2D _spawnZone;

    [SerializeField]
    private GameObject _powerPrefab;
    private List<Power> _powers = new List<Power>();

    public void Initialize(int count)
    {
        _spawnZone = transform.GetComponentInChildren<BoxCollider2D>();
        for (int i = 0; i < count; ++i)
        {
            GameObject powerGameObject = Instantiate(_powerPrefab, _spawnZone.transform.position, Quaternion.identity, _spawnZone.gameObject.transform);
            Power newPower = powerGameObject.GetComponent<Power>();
            newPower.Used = false;
            newPower.Disable();
            _powers.Add(newPower);
        }
    }

    public void AddPower(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            Vector3 spawnPosition;
            spawnPosition.x = Random.Range(_spawnZone.bounds.min.x, _spawnZone.bounds.max.x);
            spawnPosition.y = Random.Range(_spawnZone.bounds.min.y, _spawnZone.bounds.max.y);
            spawnPosition.z = _spawnZone.transform.position.z;

            for (int j = 0; j < _powers.Count; ++j)
            {
                if (_powers[j].Used == false)
                {
                    _powers[j].transform.position = spawnPosition;
                    _powers[j].Used = true;
                    _powers[j].Enable();
                    break;
                }
            }
        }
    }

    public void RemovePower(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            for (int j = 0; j < _powers.Count; ++j)
            {
                if (_powers[j].Used == true)
                {
                    _powers[j].Used = false;
                    _powers[j].Disable();
                    _powers[j].transform.position = _spawnZone.transform.position;
                    break;
                }
            }
        }
    }
}
