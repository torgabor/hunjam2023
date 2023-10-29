using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public enum State
{
    InitialMove,
    Pathing, 
    FinalMove
}
public class LocustController : MonoBehaviour
{
    public float  velocity;
    [HideInInspector]
    public Vector3 initalMovePos;
    [HideInInspector]
    public Vector3 finalMovePos; 
    [HideInInspector]
    public PrefabPathfinder pathfinder;
    [HideInInspector]
    public State state;
    
    public CropStateManager map;
    public Vector2Int startPos;
    public Vector2Int endPos;
    public MoverPrefab mover;


    // Start is called before the first frame update
    void Awake()
    {
        pathfinder = GetComponent<PrefabPathfinder>();
         mover = GetComponent<MoverPrefab>();
        state = State.InitialMove;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.InitialMove:
                transform.position = Vector3.MoveTowards(transform.position, initalMovePos, velocity * Time.deltaTime);
                //If
                if ((transform.position - initalMovePos).magnitude < 0.01f)
                {
                    Debug.Log("Initial move finished. Starting pathing");
                    state = State.Pathing;
                    mover.map = map;
                    pathfinder.start = startPos;
                    pathfinder.end = endPos;
                    mover.velocity = velocity;
                    pathfinder.StartMoving();
                }
                break;
            // case State.Pathing:
            //     break;
            case State.FinalMove:
                transform.position = Vector3.MoveTowards(transform.position, finalMovePos, velocity * Time.deltaTime);
                break;
        }
    }

    public void PathFinished()
    {
        Debug.Log("Path finished");
        state = State.FinalMove;
    }
}
