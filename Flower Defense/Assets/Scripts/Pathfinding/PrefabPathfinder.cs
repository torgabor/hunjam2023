using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class PrefabPathfinder : MonoBehaviour
{
    public CropStateManager tilemap;

    public Tile startTile;
    public Tile endTile;
    public Tile wallTile;

    public Mover mover;

    BoundsInt bounds;

    private List<Vector2Int> path;
    private AStarPrefab _aStar;

    // Start is called before the first frame update
    void Start()
    {
        _aStar = new AStarPrefab();
        var (start, end) = FindStartAndEnd();
        path = _aStar.AStarSearch(start, end, this);
        if (path == null)
        {
            Debug.Log("no path found");
        }
        else
        {
            Debug.Log($"path is ${path.Count} long");
        }

        if (mover != null)
        {
            mover.AssignPath(path);
        }
    }

    private IEnumerable<(Upgradeable tile, Vector2Int pos)> IterateTiles()
    {
        for (int xx = bounds.xMin; xx <= bounds.xMax; xx++)
        {
            for (int yy = bounds.yMin; yy <= bounds.yMax; yy++)
            {
                var tile = tilemap.GetPrefab(xx, yy);

                if (tile != null)
                {
                    yield return (tile, new Vector2Int(xx, yy));
                }
            }
        }
    }

    public void GetNeighbors(Vector2Int pos, List<Vector2Int> neighbors)
    {
        const bool skipDiag = true;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                // Skip the node itself
                if (dx == 0 && dy == 0)
                    continue;

                // Skip diagonal neighbors
                if (skipDiag && (dx != 0 && dy != 0))
                    continue;

                int newX = pos.x + dx;
                int newY = pos.y + dy;

                if (InBounds(newX, newY))
                {
                    var tileBase = tilemap.GetPrefab(newX, newY);
                    // if (IsWalkable(tileBase))
                    {
                        neighbors.Add(new Vector2Int(newX, newY));
                    }
                }
            }
        }
    }

    public (Vector2Int start, Vector2Int end) FindStartAndEnd()
    {
        Vector2Int start = Vector2Int.zero;
        Vector2Int end = Vector2Int.zero;
        foreach (var (tile, pos) in IterateTiles())
        {
            if (tile == startTile)
                start = pos;
            if (tile == endTile)
                end = pos;
        }

        return (start, end);
    }

    private bool IsWalkable(TileBase tile)
    {
        return tile != wallTile;
    }

    private bool InBounds(int newX, int newY)
    {
        return newX >= bounds.xMin && newX <= bounds.xMax && newY >= bounds.yMin && newY <= bounds.yMax;
    }

    void OnDrawGizmos()
    {
        if (path != null && path.Count > 1)
        {
            var positions = PathToPos(path);
            for (int i = 0; i < positions.Count - 1; i++)
            {
                float t = (float)i / (path.Count - 1);
                Gizmos.color = Color.Lerp(Color.green, Color.red, t);

                Vector3 startWorldPos = positions[i];
                Vector3 endWorldPos = positions[i + 1];

                Gizmos.DrawLine(startWorldPos, endWorldPos);
            }
        }
    }

    private List<Vector3> PathToPos(List<Vector2Int> mapPath)
    {
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < mapPath.Count; i++)
        {
            var pos = (Vector3Int)mapPath[i];
            positions.Add(GetCenter(pos.x, pos.y));
        }

        return positions;
    }

    private Vector3 GetCenter(int posX, int posY)
    {
        var v = tilemap.GetPrefab(posX, posY);
        return tilemap.transform.position;
    }
}