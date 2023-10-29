using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public struct NeighborInfo
{
    public Vector2Int Pos;
    public float MoveCost;
}

public class PrefabPathfinder : MonoBehaviour
{
    public CropStateManager map;

    public MoverPrefab mover;


    private List<Vector2Int> path;
    private AStarPrefab _aStar;

    public Vector2Int start;
    public Vector2Int end; 
    public bool allowDiagonal;

    private void Awake()
    {
        mover = GetComponent<MoverPrefab>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _aStar = new AStarPrefab(){allowDiag =  allowDiagonal};
        // var (start, end) = FindStartAndEnd();
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

    private IEnumerable<(CropStateManager.State tile, Vector2Int pos)> IterateTiles()
    {
        for (int xx = 0; xx <= map.Width; xx++)
        {
            for (int yy = 0; yy <= map.Height; yy++)
            {
                var tile = map.GetState(xx, yy);

                if (tile != null)
                {
                    yield return (tile, new Vector2Int(xx, yy));
                }
            }
        }
    }

    public void GetNeighbors(Vector2Int pos, List<NeighborInfo> neighbors, bool allowDiag)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                // Skip the node itself
                if (dx == 0 && dy == 0)
                    continue;

                // Skip diagonal neighbors
                if (!allowDiag && (dx != 0 && dy != 0))
                    continue;

                int newX = pos.x + dx;
                int newY = pos.y + dy;

                if (InBounds(newX, newY))
                {
                    // if (IsWalkable(tileBase))
                    var state = map.GetState(newX, newY);
                    var ni = new NeighborInfo()
                    {
                        Pos = new Vector2Int(newX, newY),
                        MoveCost = state.GetMovementMultiplier()
                    };
                    neighbors.Add(ni);
                }
            }
        }
    }

    // public (Vector2Int start, Vector2Int end) FindStartAndEnd()
    // {
    //     Vector2Int start = Vector2Int.zero;
    //     Vector2Int end = Vector2Int.zero;
    //     foreach (var (tile, pos) in IterateTiles())
    //     {
    //         // if (tile == startTile)
    //         //     start = pos;
    //         // if (tile == endTile)
    //         //     end = pos;
    //     }
    //
    //     return (start, end);
    // }

    private bool InBounds(int newX, int newY)
    {
        return newX >= 0 && newX < map.Width && newY >= 0 && newY < map.Height;
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
        var state = map.GetState(posX, posY);
        return state.WorldPos;
    }
}