using System;
using UnityEngine;
using UnityEngine.Tilemaps;


[Serializable]
public class GrassState
{
    public float WaterLevel;
}

public class GrassManager : MonoBehaviour
{
    public float[] WaterThresholds;

    public GrassState[] Grid;
    public int Width;
    public int Height;
    public TileBase GrassTile;
    public Tilemap tilemap;

    public int GetSpriteLevel(float waterLevel)
    {
        for (int i = 0; i < WaterThresholds.Length; i++)
        {
            if (waterLevel < WaterThresholds[i])
                return i;
        }

        return WaterThresholds.Length;
    }

    public GrassState GetGrassState(int x, int y)
    {
        return Grid[x + y * Width];
    }

    public void Start()
    {
        SetupTileMap();
    }

    public void SetupTileMap()
    {
        Grid = new GrassState[Width * Height];
        for (int i = 0; i < Grid.Length; i++)
        {
            Grid[i] = new GrassState();
        }

        for (int xx = 0; xx < Width; xx++)
        {
            for (int yy = 0; yy < Height; yy++)
            {
                var pos = new Vector3Int(xx, yy);
                tilemap.SetTile(pos, GrassTile);
                UpdateTile(xx, yy);
            }
        }
    }


    public void UpdateTileMap()
    {
        for (int xx = 0; xx < Width; xx++)
        {
            for (int yy = 0; yy < Height; yy++)
            {
                UpdateTile(xx, yy);
            }
        }
    }

    public void AddWater(int x, int y, float amount)
    {
        var grassState = GetGrassState(x, y);
        grassState.WaterLevel += amount;
        UpdateTile(x, y);
    }

    public void UpdateTile(int xx, int yy)
    {
        var grassState = GetGrassState(xx, yy);
        var waterLevel = GetSpriteLevel(grassState.WaterLevel);
        var pos = new Vector3Int(xx, yy);
        tilemap.SetAnimationFrame(pos, waterLevel);
        tilemap.SetTileAnimationFlags(pos, TileAnimationFlags.PauseAnimation);
    }

    public float waterSpeed =10;
    public float waterRadius = 2f;
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float amount = Time.deltaTime * waterSpeed;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Vector3Int gridPosition = tilemap.WorldToCell(mousePosition);
            for (int xx = 0; xx < Width; xx++)
            {
                for (int yy = 0; yy < Height; yy++)
                {
                    var tilePos = tilemap.CellToWorld(new Vector3Int(xx, yy));
                    var dist = ((Vector3)mousePosition - tilePos).magnitude;
                    if (dist < waterRadius)
                    {
                        AddWater(xx, yy, amount);
                     //   Debug.Log(gridPosition);
                    }

                }
            }
         

        }
    }
}