using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController active;

    private int selectedIndex = 0;

    private UI_Tile[] tiles = { new UI_Tile_button(), new UI_Tile_lamp(), new UI_Tile_wire() };

    public void Awake()
    {
        active = this;
    }

    private void Update()
    {
        inventoryControls();
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
}
