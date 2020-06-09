
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    public static GridSystem Instance;
    public Camera CurrentCamera;
    public Tilemap MainTilemap;

    private LifeGraph _mapGraph;

    public Vector3Int[] offsets = { new Vector3Int(1, 0, 0), new Vector3Int(1, 1, 0),
                                    new Vector3Int(0, 1, 0), new Vector3Int(-1, 1, 0),
                                    new Vector3Int(-1, 0, 0), new Vector3Int(-1, -1, 0),
                                    new Vector3Int(0, -1, 0), new Vector3Int(1, -1, 0)};

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }
        InitializeGraph();
    }

    private void Start()
    {

    }

/*
 * Graph section
 */
 
    public void InitializeGraph()
    {
        _mapGraph = new LifeGraph();
        MainTilemap.CompressBounds();

        //analyze tilemap and build map graph
        foreach(Vector3Int pos in MainTilemap.cellBounds.allPositionsWithin)
        {
            Tile tile = MainTilemap.GetTile<Tile>(pos);
            if (tile != null)
            {
                LifeNode centralTileNode = _mapGraph.GetNode(pos) as LifeNode;
                if (centralTileNode == null)
                {
                    centralTileNode = new LifeNode();
                    centralTileNode.Coords = pos;
                    centralTileNode.Status = LifeNode.NodeStatus.Empty;
                    centralTileNode.NewStatus = LifeNode.NodeStatus.Empty;
                    _mapGraph.AddNode(centralTileNode);
                }
            }
        }
    }

    public Node GetNode(Vector3Int coords)
    {
        return _mapGraph.GetNode(coords);
    }

    public List<LifeNode> GetNearNodes(Vector3Int pos)
    {
        List<LifeNode> nearNodes = new List<LifeNode>();
        for (int i = 0; i < 8; ++i)
        {
            LifeNode node = _mapGraph.GetNode(pos + offsets[i]) as LifeNode;
            if (node != null)
                nearNodes.Add(node);
        }
        return nearNodes;
    }

    public void AddLifeToGraph(LifeDot life)
    {
        Vector3Int coords = GetTilemapCoordsFromWorld(life.transform.position);
        LifeNode node = _mapGraph.GetNode(coords) as LifeNode;
        node.AddLife(life);
    }

    /*
     * Tilemap section
     */

    public void PrintTileInfo(Vector3Int cellPosition)
    {
        Tile tile = MainTilemap.GetTile(cellPosition) as Tile;
        if (tile != null)
        {
            Debug.Log($"Tile at position ({cellPosition.x}, {cellPosition.y}) exists\n");
            Node node = _mapGraph.GetNode(cellPosition);
            Debug.Log($"Connections count: {node.Connections.Count}");
        }
        else
            Debug.Log($"Tile at position ({cellPosition.x}, {cellPosition.y}) does not exist");
    }

    public Vector3Int GetTilemapCoordsFromWorld(Vector3 worldCoords)
    {
        return MainTilemap.WorldToCell(worldCoords);
    }

    public Vector3Int GetTilemapCoordsFromScreen(Vector3 screenCoords)
    {
        Ray ray = CurrentCamera.ScreenPointToRay(screenCoords);
        Vector3 worldPosition = ray.GetPoint(-ray.origin.z / ray.direction.z);
        return MainTilemap.WorldToCell(worldPosition);
    }

    public Vector3 GetWorldCoordsFromTilemap(Vector3Int tilemapCoords)
    {
        return MainTilemap.GetCellCenterWorld(tilemapCoords);
    }

    public Vector2 GetRelativePointPositionInTile(Vector3Int cellCoords, Vector3 pointCoords)
    {
        Vector3 cellWorldCenter = MainTilemap.GetCellCenterWorld(cellCoords);
        Vector3 cellSize = MainTilemap.cellSize;
        //offset center to left bottom corner of tile
        cellWorldCenter -= cellSize / 2;

        float x = (pointCoords.x - cellWorldCenter.x) / cellSize.x;
        float y = (pointCoords.y - cellWorldCenter.y) / cellSize.y;
        return new Vector2(x, y);
    }
}
