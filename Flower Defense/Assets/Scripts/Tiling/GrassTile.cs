using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Grass Tile", menuName = "Tiles/Grass Tile")]
public class GrassTile : Tile 
{
    public Sprite spriteA;
    public Sprite spriteB;

    [SerializeField]
    public float Value;
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = Value>50 ? spriteA : spriteB;

        // You can also use 'someField' to do additional custom logic.
    }


    // You can override more TileBase methods if needed
}