using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class LocustSpawner :  PathManager
{
    public int SpawnColumn;
    public int[] SpawnRows;
    public float SpawnTimeMinSeconds;
    public float SpawnTimeMaxSeconds;
    public Vector2Int endPos;

    public Vector3 locustOffset;
    
    public GameObject LocustPrefab;

    [SerializeField] private long NextSpawnEvent;

    public int MaxLocusts;

    public List<LocustController> Locusts;
    public CropStateManager map;
    public Transform finalTarget;
    
    // Update is called once per frame
    void Update()
    {
        var dtTicks = new DateTime(NextSpawnEvent);
        var now = DateTime.UtcNow;
        if (dtTicks < now)
        {
            SpawnLocust();
            var spawnDelay = UnityEngine.Random.Range(SpawnTimeMinSeconds, SpawnTimeMaxSeconds);
            var nextSpawnEvent = now + TimeSpan.FromSeconds(spawnDelay);
            NextSpawnEvent = nextSpawnEvent.Ticks;
        }
    }

    private void SpawnLocust()
    {
        CleanupReferences();
        if (Locusts.Count >= MaxLocusts)
        {
            return;
        }

        var row = SpawnRows[UnityEngine.Random.Range(0, SpawnRows.Length)];
        var pos = new Vector2Int(SpawnColumn, row );
        var pathPos = map.GetState(pos).WorldPos;
        var worldPos = pathPos + locustOffset;
        
        var locust = Instantiate(LocustPrefab, worldPos, Quaternion.identity).GetComponent<LocustController>();
        locust.map = map;
        locust.startPos = pos;
        locust.initalMovePos = pathPos;
        locust.finalMovePos = finalTarget.position;
        locust.endPos = endPos;
        var locustMover = locust.GetComponent<MoverPrefab>();
        locustMover.PathManager = this;
        
        Locusts.Add(locust);
    }

    private void CleanupReferences()
    {
        for (int i = 0; i < Locusts.Count; i++)
        {
            if (IsDestroyed(Locusts[i].gameObject))
            {
                Locusts.RemoveAt(i--);
            }
        }
    }
    
    public override void PathFinished(MoverPrefab moverPrefab)
    {
        base.PathFinished(moverPrefab);
        var locust = moverPrefab.GetComponent<LocustController>();
        locust.PathFinished();
    }

    public static bool IsDestroyed(GameObject target)
    {
        // Checks whether a Unity object is not actually a null reference,
        // but a rather destroyed native instance.

        return !ReferenceEquals(target, null) && target == null;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(map == null)
            return;
        foreach (var row in SpawnRows)
        {
            var pos = new Vector2Int(SpawnColumn, row);
            var localPos = new Vector3(map.prefabSize.x * pos.x, map.prefabSize.y * pos.y);
            
            var pathPos = map.transform.TransformPoint(localPos);
            var worldPos = pathPos + locustOffset;
            Gizmos.DrawSphere(worldPos, 0.1f);
        }


    }
}