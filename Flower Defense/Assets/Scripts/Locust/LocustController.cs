using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public enum State
{
    InitialMove,
    Pathing,
    FinalMove
}

public class LocustController : MonoBehaviour
{
    public float velocity;
    [HideInInspector] public Vector3 initalMovePos;
    [HideInInspector] public Vector3 finalMovePos;
    [HideInInspector] public PrefabPathfinder pathfinder;
    [HideInInspector] public State state;

    public CropStateManager map;
    public Vector2Int startPos;
    public Vector2Int endPos;
    public MoverPrefab mover;
    public AudioSource audioSource;
    [SerializeField] public List<AudioClip> _flyClips;


    // Start is called before the first frame update
    void Awake()
    {
        pathfinder = GetComponent<PrefabPathfinder>();
        mover = GetComponent<MoverPrefab>();
        state = State.InitialMove;

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = _flyClips[Random.Range(0, _flyClips.Count)];
        audioSource.volume = 0.05f;
        audioSource.pitch = Random.Range(0.75f, 1.25f);
    }

    private void Start()
    {
        audioSource.Play();
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
                if ((transform.position - finalMovePos).magnitude < 0.01f)
                {
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                }
                break;
        }
    }

    public void PathFinished()
    {
        Debug.Log("Path finished");
        state = State.FinalMove;
    }
}