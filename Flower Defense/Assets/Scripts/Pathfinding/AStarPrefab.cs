using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinding
{
    public class AStarPrefab
    {
        public class Node : IEquatable<Node>
        {
            public Vector2Int pos;
            public float gCost = 0;  // Cost from start to this node
            public Node src;  // Parent node for path tracing

            public bool Equals(Node other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return pos.Equals(other.pos);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Node)obj);
            }

            public override int GetHashCode()
            {
                return pos.GetHashCode();
            }
        }
        
        List<Node> openSet = new ();
        HashSet<Vector2Int> closedSet = new ();
        private List<NeighborInfo> neighbors = new();
        private Dictionary<Vector2Int, Node> nodes = new();
        public bool allowDiag = true;
        
        public List<Vector2Int> AStarSearch(Vector2Int startPos, Vector2Int endPos, PrefabPathfinder pathfinder)
        {
             float FCost(Node a)
             {
                 var hCost = HCost(a);
                 return hCost + a.gCost;
             }
             float HCost(Node a)
             {
                 var hCost = GetDistance(a.pos, endPos);
                 return hCost;
             }

             Node GetNode(Vector2Int pos) =>
                 nodes.TryGetValue(pos, out var v) ? v : nodes[pos] = new Node() { pos = pos };
            nodes.Clear();
            openSet.Clear();
            closedSet.Clear();
            var startNode = GetNode(startPos);
            openSet.Add(startNode);
    
            while (openSet.Count > 0)
            {
                // Find node with lowest fCost
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (FCost(openSet[i]) < FCost(currentNode))
                    {
                        currentNode = openSet[i];
                    }
                }

                // Remove current from openSet and add to closedSet
                openSet.Remove(currentNode);
                closedSet.Add(currentNode.pos);

                // Found the target
                if (currentNode.pos == endPos)
                {
                    return RetracePath(startPos, currentNode);
                }

                // Check all neighbors
                neighbors.Clear();
                pathfinder.GetNeighbors(currentNode.pos, neighbors, allowDiag);
                foreach (var ni in neighbors)
                {
                    if (closedSet.Contains(ni.Pos))
                    {
                        continue;
                    }

                    var neighbor = GetNode(ni.Pos);
                    float newGCost = currentNode.gCost + GetDistance(currentNode.pos, ni.Pos)*ni.MoveCost;
                    if (newGCost < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newGCost;
                        neighbor.src = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                   
                }
            }

            // Return null if no path is found
            return null;
        }

        private  float GetDistance(Vector2Int a, Vector2Int b)
        {
            
            // // Use basic manhattan distance
            // 
            //Allow diagonal movements
            if (allowDiag)
            {
                return (a - b).magnitude;
            }
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        public List<Vector2Int> RetracePath(Vector2Int startPos, Node endNode)
        {
            var path = new List<Vector2Int>();
            Node currentNode = endNode;
    
            while (currentNode.pos != startPos)
            {
                path.Add(currentNode.pos);
                currentNode = currentNode.src;
            }
            path.Add(startPos);
            path.Reverse();
            return path;
        }
    }
}