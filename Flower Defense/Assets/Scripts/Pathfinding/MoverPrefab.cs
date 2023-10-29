using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Vector2 = System.Numerics.Vector2;


public class MoverPrefab : MonoBehaviour
{
    public CropStateManager map;

    public float velocity;
    public float angularVelocity;
    [HideInInspector] public int currentTile;
    [HideInInspector] public Vector3 currentHeading;
    [HideInInspector] public Vector3 nextWaypoint;
    [HideInInspector] public Vector3 nextHeading;
    [HideInInspector] public Vector3 currentPos;
    [HideInInspector] public List<Vector2Int> path;
    public bool isMoving = true;

    public void AssignPath(List<Vector2Int> path)
    {
        this.path = path;
        currentTile = 0;
        
        nextWaypoint = map.GetState(path[1]).WorldPos;
        currentPos = map.GetState(path[0]).WorldPos;
        currentHeading = nextWaypoint - currentPos;
        nextHeading = currentHeading;
        UpdateTransform();
    }
    
    public void Update()
    {
        if (path == null || currentTile >= path.Count)
        {
            return;
        }

        if (!isMoving)
        {
            return;
        }

        var tile = path[currentTile];
        var movementMultiplier = map.GetState(tile).GetMovementMultiplier();
        // var currentCell = tilemap.WorldToCell(currentPos);
        var dt = Time.deltaTime;
       // var heading = GetHeading(currentTile);
        var nextPos = Vector3.MoveTowards(currentPos, nextWaypoint, movementMultiplier * velocity * dt);
        // var nextCell = tilemap.WorldToCell(nextPos);

        currentHeading = Vector3.RotateTowards(this.currentHeading, nextHeading, angularVelocity * dt, 10000f);
        // Debug.Log($"target heading: {heading} current heading: {currentHeading}");
        currentPos = nextPos;
        //set the transform position and rotation
        currentPos = nextPos;
        UpdateTransform();
        if (Vector3.Distance(nextWaypoint,currentPos)< 0.01f )
        {
            
            currentTile++;
            nextHeading = (map.GetState(path[currentTile]).WorldPos - currentPos).normalized;
            nextWaypoint = map.GetState(path[currentTile]).WorldPos;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(currentPos, currentPos + currentHeading);
    }

    private void UpdateTransform()
    {
        // var rot = Quaternion.LookRotation(currentHeading, Vector3.Cross(new Vector3(0,0,1),currentHeading));
        float angle = Mathf.Atan2(currentHeading.y, currentHeading.x) * Mathf.Rad2Deg;
        var rot = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.SetPositionAndRotation(currentPos, rot);
        // transform.position = currentPos;
        // transform.LookAt(currentHeading,Vector3.forward);
    }
}