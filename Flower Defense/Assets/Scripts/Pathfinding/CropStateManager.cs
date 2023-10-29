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
        public Upgradeable Upgradeable;
        public Destroyable Destroyable;
    }

    [HideInInspector] public State[] Grid;
    public int Width;
    public int Height;
    public GameObject grassPrefab;
    public Vector2 prefabSize;
    public Vector2 prefabRandomOffset;
    public float[] MoveMulitplierByLevel;
    [Range(0,1)]
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
                    URandom.Range(-0.5f,0.5f) * prefabRandomOffset.x,
                    URandom.Range(-0.5f,0.5f) * prefabRandomOffset.y);
                pos += offset;
                var go = Instantiate(grassPrefab, transform);
                go.transform.localPosition = pos;
                var upgradeable = go.GetComponentInChildren<Upgradeable>();
                var destroyable = go.GetComponentInChildren<Destroyable>();
                destroyable.stateManager = this;
                upgradeable.manager = this;
                var level = GetCropLevel();
                upgradeable.CurrentLvl = level;
                Grid[idx] = new State()
                {
                    Destroyable = destroyable,
                    Upgradeable = upgradeable
                };
            }
        }
    }

    public State GetState(int x, int y)
    {
        return Grid[x + y * Width];
    }

    public float GetMovementMultiplier(int x, int y)
    {
        var state = GetState(x, y);
        if (state.Destroyable.Hp <= 0)
        {
            return 1f;
        }

        var level = state.Upgradeable.CurrentLvl;

        return MoveMulitplierByLevel[level];
    }

    public void LevelChanged(Upgradeable upgradeable)
    {
    }
}