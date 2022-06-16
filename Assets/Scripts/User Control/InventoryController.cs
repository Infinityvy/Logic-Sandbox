using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController active;

    public LevelMesh inventoryMesh;

    private int selectedIndex = 0;

    private UI_Tile[] tiles = { new UI_Tile_button(), new UI_Tile_lamp(), new UI_Tile_wire() };

    private Tile[,] invMeshTiles;

    private void Awake()
    {
        active = this;
    }

    private void Start()
    {
        invMeshTiles = new Tile[1, tiles.Length];

        for(int y = 0; y < tiles.Length; y++)
        {
            invMeshTiles[0, y] = tiles[y].getTile(new Vector2Int(0, y));
        }

        inventoryMesh.generateMesh(invMeshTiles, 10);
    }

    private void Update()
    {
        inventoryControls();
        inventoryDisplay();
    }

    private void inventoryControls()
    {
        for(int i = 0; i < InputSettings.numbers.Length && i < tiles.Length; i++)
        {
            if(Input.GetKeyDown(InputSettings.numbers[i]))
            {
                selectedIndex = i;
                break;
            }
        }
    }

    public Tile GetSelectedTile(Vector2Int position)
    {
        return tiles[selectedIndex].getTile(position);
    }

    private void inventoryDisplay()
    {

    }
}
