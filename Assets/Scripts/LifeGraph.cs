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

    public void AddLife(LifeDot life)
    {
        if (ObjectsOnTile.Count == 0)
        {
            ObjectsOnTile.Add(life.gameObject);
            Status = NodeStatus.Occupied;
        }
    }

    public LifeDot RemoveLife()
    {
        if (ObjectsOnTile.Count != 0)
        {
            GameObject lifeObject = ObjectsOnTile[0];
            LifeDot life = lifeObject.GetComponent<LifeDot>();
            ObjectsOnTile.Clear();
            Status = NodeStatus.Empty;
            return life;
        }
        return null;
    }

    public LifeDot GetLife()
    {
        if (ObjectsOnTile.Count > 0)
        {
            return ObjectsOnTile[0].GetComponent<LifeDot>();
        }
        return null;
    }
}

public class LifeGraph : MapGraph
{

}
