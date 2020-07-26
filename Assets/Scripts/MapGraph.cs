using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    //general variables
    public Vector3Int Coords = new Vector3Int();
    public List<Connection> Connections = new List<Connection>();

    //List of objects that stay on tile (characters, effects and so on)
    public List<GameObject> ObjectsOnTile = new List<GameObject>();

    public bool AddConnection(Connection connection)
    {
        foreach (var oldConnection in Connections)
        {
            if (oldConnection.StartNode == connection.StartNode && oldConnection.EndNode == connection.EndNode)
                return false;
        }
        Connections.Add(connection);
        return true;
    }

    public bool AddConnection(Node to)
    {
        foreach(var connection in Connections)
        {
            if (connection.StartNode == this && connection.EndNode == to)
                return false;
        }
        Connection newConnection = new Connection(this, to);
        Connections.Add(newConnection);
        return true;
    }

    public void RemoveConnection(Node to)
    {
        for(int i = 0; i < Connections.Count; ++i)
        {
            var connection = Connections[i];
            if (connection.EndNode == to)
            {
                Connections.Remove(connection);
                --i;
            }
        }
    }
}

public class Connection
{
    //public float Cost;
    public Node StartNode;
    public Node EndNode;

    public Connection()
    {
        //Cost = 0;
        StartNode = null;
        EndNode = null;
    }

    public Connection(Node from, Node to)
    {
        //Cost = cost;
        StartNode = from;
        EndNode = to;
    }
}

public class MapGraph
{
    //key - coordinates in form (x,y)
    protected Dictionary<string, Node> _nodeGraph;

    public MapGraph()
    {
        _nodeGraph = new Dictionary<string, Node>();
    }

    public void GetNodeCoordinates(string nodeKey, out int x, out int y)
    {
        char[] splitters = { '(', ',', ')' };
        string[] coords = nodeKey.Split(splitters);
        x = int.Parse(coords[0]);
        y = int.Parse(coords[1]);
    }

    public string CreateNodeKeyFromCoordinates(int x, int y)
    {
        //return $"({x},{y})";
        return "(" + x.ToString() + "," + y.ToString() + ")";
    }

    public Node GetNode(Vector3Int coords)
    {
        string key = CreateNodeKeyFromCoordinates(coords.x, coords.y);
        if (_nodeGraph.ContainsKey(key))
            return _nodeGraph[key];
        return null;
    }

    public Node GetNode(string coords)
    {
        Node node = null;
        bool exist = _nodeGraph.TryGetValue(coords, out node);
        return node;
    }

    public bool AddNode(Node node)
    {
        string key = CreateNodeKeyFromCoordinates(node.Coords.x, node.Coords.y);
        if (_nodeGraph.ContainsKey(key))
            return false;
        _nodeGraph.Add(key, node);
        return true;
    }

    public bool Contains(string coords)
    {
        return _nodeGraph.ContainsKey(coords);
    }

    public bool Contains(Node node)
    {
        string key = CreateNodeKeyFromCoordinates(node.Coords.x, node.Coords.y);
        return _nodeGraph.ContainsKey(key);
    }
}
