using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeInfoPanel : MonoBehaviour
{
    [SerializeField]
    private Text _positionText;
    [SerializeField]
    private Text _yearsText;

    protected LifeNode Node;

    public void UpdateInfo()
    {
        if (Node == null)
            return;
        if (Node.GetLife() == null)
        {
            _yearsText.gameObject.SetActive(false);
        }
        else
        {
            _yearsText.gameObject.SetActive(true);
            _yearsText.text = Node.GetLife().Age.ToString();
        }
    }

    public void SetNode(LifeNode node)
    {
        Node = node;
        _positionText.text = $"{Node.Coords.x}, {Node.Coords.y}";
        UpdateInfo();
    }
}
