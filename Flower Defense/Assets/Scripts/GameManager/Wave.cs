using UnityEngine;

[CreateAssetMenu()]
public class Wave : ScriptableObject
{
    public float SpawnTimeMinSeconds;
    public float SpawnTimeMaxSeconds;
    public int SpawnCount;
    public int MaxLocusts;
}