using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tile
{
    public virtual int spriteID { get;} //same as Tile's simpleID
    public byte[] metadata { get; set; } = { 0, 0, 0, 0 , 1};

    public virtual Tile getTile(Vector2Int position)
    {
        return null;
    }

    public virtual void setWire(int index, byte state)
    {
        metadata[index] = state;
    }
}

public class UI_Tile_wire : UI_Tile
{
    public override int spriteID { get; } = 0; //same as Tile's simpleID

    public override Tile getTile(Vector2Int position)
    {
        Tile tile = new Tile_wire(position, metadata);
        tile.updatePowered();
        return tile;
    }

    public void setDelay(byte delay)
    {
        metadata[4] = delay;
    }

    public byte getDelay()
    {
        return metadata[4];
    }
}

public class UI_Tile_lamp : UI_Tile
{
    public override int spriteID { get; } = 1; //same as Tile's simpleID
    public override Tile getTile(Vector2Int position)
    {
        Tile tile = new Tile_lamp(position, metadata);
        tile.updatePowered();
        return tile;
    }
}

public class UI_Tile_button : UI_Tile
{
    public override int spriteID { get; } = 2; //same as Tile's simpleID
    public override Tile getTile(Vector2Int position)
    {
        return new Tile_button(position, metadata);
    }

    public override void setWire(int index, byte state)
    {
        if (state == 1) state = 2;
        base.setWire(index, state);
    }
}
