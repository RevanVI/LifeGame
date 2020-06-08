using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeNode: Node
{
    public enum NodeStatus
    {
        Empty = 0,
        Occupied = 1
    }

    public NodeStatus Status;
    public NodeStatus NewStatus;
}

public class LifeGraph : MapGraph
{

}
