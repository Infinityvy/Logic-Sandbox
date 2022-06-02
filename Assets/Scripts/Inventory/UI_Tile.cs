using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tile
{
    public virtual Tile getTile(Vector2Int position)
    {
        return null;
    }
}

public class UI_Tile_wire : UI_Tile
{
    public override Tile getTile(Vector2Int position)
    {
        Tile tile = new Tile_wire(position);
        tile.updatePowered();
        return tile;
    }
}

public class UI_Tile_lamp : UI_Tile
{
    public override Tile getTile(Vector2Int position)
    {
        Tile tile = new Tile_lamp(position);
        tile.updatePowered();
        return tile;
    }
}

public class UI_Tile_button : UI_Tile
{
    public override Tile getTile(Vector2Int position)
    {
        return new Tile_button(position);
    }
}
