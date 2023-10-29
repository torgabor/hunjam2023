using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using URandom = UnityEngine.Random;


public class CropStateManager : StateManager
{
    [Serializable]
    public class State
    {
        [SerializeField] private CropStateManager Owner;
        public Upgradeable Upgradeable;
        public Destroyable Destroyable;
        public Vector2Int Pos;
        public Vector3 WorldPos;

        public State(CropStateManager owner, Upgradeable upgradeable, Destroyable destroyable, Vector2Int pos,
            Vector3 worldPos)
        {
            Owner = owner;
            Upgradeable = upgradeable;
            Destroyable = destroyable;
            Pos = pos;
            WorldPos = worldPos;
        }

        public float GetMovementMultiplier()
        {
            if (Destroyable.Hp <= 0)
            {
                return 1f;
            }

            var level = Upgradeable.CurrentLvl;

            return Owner.MoveMulitplierByLevel[level];
        }

        public Vector3 GetCenterWorld()
        {
            return Upgradeable.transform.position;
        }
    }

    [HideInInspector] public State[] Grid;
    public int Width;
    public int Height;
    public GameObject grassPrefab;
    public Vector2 prefabSize;
    public Vector2 prefabRandomOffset;
    public float[] MoveMulitplierByLevel;
    public int[] InitialLevelChance;

    int GetCropLevel()
    {
        if (InitialLevelChance == null || InitialLevelChance.Length == 0)
            return 0;
        var sum = InitialLevelChance.Sum();
        var val = URandom.Range(0, sum);
        for (int i = 0; i < InitialLevelChance.Length; i++)
        {
            var elem = InitialLevelChance[i];
            if (val < elem)
                return i;
            val -= elem;
        }

        return InitialLevelChance.Length - 1;
    }

    public void Start()
    {
        SetupMap();
    }

    public void SetupMap()
    {
        Grid = new State[Width * Height];
        int idx = 0;
        for (int xx = 0; xx < Width; xx++)
        {
            for (int yy = 0; yy < Height; yy++, idx++)
            {
                var pos = new Vector3(prefabSize.x * xx, prefabSize.y * yy);
                var offset = new Vector3(
                    URandom.Range(-0.5f, 0.5f) * prefabRandomOffset.x,
                    URandom.Range(-0.5f, 0.5f) * prefabRandomOffset.y);
                pos += offset;
                var go = Instantiate(grassPrefab, transform);
                go.transform.localPosition = pos;
                var upgradeable = go.GetComponentInChildren<Upgradeable>();
                var destroyable = go.GetComponentInChildren<Destroyable>();
                destroyable.stateManager = this;
                upgradeable.manager = this;
                var level = GetCropLevel();
                upgradeable.CurrentLvl = level;
                var localPos = pos;//+ (Vector3)prefabSize * 0.5f;
                var worldPos = this.transform.TransformPoint(localPos);
                Grid[idx] = new State(this, upgradeable, destroyable, new Vector2Int(xx, yy), worldPos);
            }
        }
    }

    public State GetState(int x, int y)
    {
        return Grid[x + y * Width];
    }

    public State GetState(Vector2Int v)
    {
        return Grid[v.x + v.y * Width];
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        var size = new Vector3(prefabSize.x * Width, prefabSize.y * Height, 1);
        Gizmos.DrawWireCube(transform.position + size * 0.5f - (Vector3)(prefabSize * 0.5f), size);
    }

}