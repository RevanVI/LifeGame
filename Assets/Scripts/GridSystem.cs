
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
                Node centralTileNode = _mapGraph.GetNode(pos);
                if (centralTileNode == null)
                {
                    centralTileNode = new Node();
                    centralTileNode.Coords = pos;
                    _mapGraph.AddNode(centralTileNode);
                }
                for (int i = 0; i < 8; ++i)
                {
                    Vector3Int currentTileLocation = pos + offsets[i];
                    Tile offsetTile = MainTilemap.GetTile<Tile>(currentTileLocation);
                    if (offsetTile != null)
                    {
                        Node offsetTileNode = _mapGraph.GetNode(currentTileLocation);
                        if (offsetTileNode == null)
                        {
                            offsetTileNode = new Node();
                            offsetTileNode.Coords = currentTileLocation;
                            _mapGraph.AddNode(offsetTileNode);
                        }
                        centralTileNode.AddConnection(offsetTileNode);
                    }
                }
            }
        }
    }

    public Node GetNode(Vector3Int coords)
    {
        return _mapGraph.GetNode(coords);
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

    public Vector3Int GetTilemapCoordsFromWorld(Tilemap tilemap, Vector3 worldCoords)
    {
        return tilemap.WorldToCell(worldCoords);
    }

    public Vector3Int GetTilemapCoordsFromScreen(Tilemap tilemap, Vector3 screenCoords)
    {
        Ray ray = CurrentCamera.ScreenPointToRay(screenCoords);
        Vector3 worldPosition = ray.GetPoint(-ray.origin.z / ray.direction.z);
        return tilemap.WorldToCell(worldPosition);
    }

    public Vector3 GetWorldCoordsFromTilemap(Tilemap tilemap, Vector3Int tilemapCoords)
    {
        return tilemap.CellToWorld(tilemapCoords);
    }

    public Vector2 GetRelativePointPositionInTile(Tilemap tilemap, Vector3Int cellCoords, Vector3 pointCoords)
    {
        Vector3 cellWorldCenter = tilemap.GetCellCenterWorld(cellCoords);
        Vector3 cellSize = tilemap.cellSize;
        //offset center to left bottom corner of tile
        cellWorldCenter -= cellSize / 2;

        float x = (pointCoords.x - cellWorldCenter.x) / cellSize.x;
        float y = (pointCoords.y - cellWorldCenter.y) / cellSize.y;
        return new Vector2(x, y);
    }
}
