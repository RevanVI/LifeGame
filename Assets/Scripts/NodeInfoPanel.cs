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

    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private Button _spawnButton;
    [SerializeField]
    private Button _removeButton;

    private LifeNode _node;
    public LifeNode NodeRef
    {
        get { return _node; }
    }

    public IntEvent OnButtonsClick;

    public NodeInfoPanel()
    {
        OnButtonsClick = new IntEvent();
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(ProcessCloseClick);
        _spawnButton.onClick.AddListener(ProcessSpawnClick);
        _removeButton.onClick.AddListener(ProcessRemoveClick);
    }

    public void UpdateInfo()
    {
        if (_node == null)
            return;
        if (_node.GetLife() == null)
        {
            _yearsText.gameObject.SetActive(false);
        }
        else
        {
            _yearsText.gameObject.SetActive(true);
            _yearsText.text = _node.GetLife().Age.ToString();
        }
    }

    public void SetNode(LifeNode node)
    {
        _node = node;
        _positionText.text = $"{_node.Coords.x}, {_node.Coords.y}";
        UpdateInfo();
    }

    public void Close()
    {
        _node = null;
        gameObject.SetActive(false);
    }

    private void ProcessCloseClick()
    {
        OnButtonsClick.Invoke(0);
    }

    private void ProcessSpawnClick()
    {
        OnButtonsClick.Invoke(1);
    }

    private void ProcessRemoveClick()
    {
        OnButtonsClick.Invoke(2);
    }
}
