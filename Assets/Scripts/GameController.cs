using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessStep()
    {
        foreach (Vector3Int pos in GridSystem.Instance.MainTilemap.cellBounds.allPositionsWithin)
        {
            LifeNode currentNode = GridSystem.Instance.GetNode(pos) as LifeNode;
            List<LifeNode> nearNodes = GridSystem.Instance.GetNearNodes(pos);
            int neigboursCount = GetNeighboursCount(nearNodes);

            if (neigboursCount == 3 ||
                (currentNode.Status == LifeNode.NodeStatus.Occupied && neigboursCount == 2))
                currentNode.NewStatus = LifeNode.NodeStatus.Occupied;
            else
                currentNode.NewStatus = LifeNode.NodeStatus.Empty;
        }
    }

    private int GetNeighboursCount(List<LifeNode> nearNodes)
    {
        int count = 0;
        foreach (var node in nearNodes)
        {
            if (node.Status == LifeNode.NodeStatus.Occupied)
                ++count;
        }
        return count;
    }
}

