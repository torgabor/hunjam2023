using System;
using UnityEngine;
using UnityEngine.Tilemaps;


public class CropStateManager : MonoBehaviour
{
    public Upgradeable[] Grid;
    public int Width;
    public int Height;
    public GameObject grassPrefab;
    public Vector2 prefabSize;

    public void Start()
    {
        SetupMap();
    }

    public void SetupMap()
    {
        Grid = new Upgradeable[Width * Height];
        int idx = 0;
        for (int xx = 0; xx < Width; xx++)
        {
            for (int yy = 0; yy < Height; yy++,idx++)
            {
                var pos = new Vector3(prefabSize.x * xx, prefabSize.y * yy);
                var go = Instantiate(grassPrefab, transform);
                go.transform.localPosition = pos;
                var upgradeable = go.GetComponentInChildren<Upgradeable>();
                upgradeable.manager = this;
                Grid[idx] = upgradeable;
                
            }
        }
    }

    public Upgradeable GetPrefab(int x, int y)
    {
        return Grid[x + y * Width];
    } 

    public void LevelChanged(Upgradeable upgradeable)
    {
    }
}