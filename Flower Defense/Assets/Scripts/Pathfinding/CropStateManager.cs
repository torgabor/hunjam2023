using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;


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

    public void Start()
    {
        SetupMap();
    }

    public void SetupMap()
    {
        Grid = new State[Width * Height];
        int idx = 0;
        var random = new Random();
        for (int xx = 0; xx < Width; xx++)
        {
            for (int yy = 0; yy < Height; yy++, idx++)
            {
                var pos = new Vector3(prefabSize.x * xx, prefabSize.y * yy);
                var offset = new Vector3(
                    ((float)random.NextDouble() - 0.5f) * prefabRandomOffset.x,
                    ((float)random.NextDouble() - 0.5f) * prefabRandomOffset.y);
                pos += offset;
                var go = Instantiate(grassPrefab, transform);
                go.transform.localPosition = pos;
                var upgradeable = go.GetComponentInChildren<Upgradeable>();
                var destroyable = go.GetComponentInChildren<Destroyable>();
                destroyable.stateManager = this;
                upgradeable.manager = this;
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