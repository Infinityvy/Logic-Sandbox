using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tile
{
    public virtual int spriteID { get;}
    protected byte[] metadata { get; set; } = { 0, 0, 0, 0 , 10};
    public byte[] simple_metadata { get; set; } = { 0, 0, 0, 0};

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
    public override int spriteID { get; } = 0;

    public override Tile getTile(Vector2Int position)
    {
        Tile tile = new Tile_wire(position, metadata);
        tile.updatePowered();
        return tile;
    }

    public override void setWire(int index, byte state)
    {
        simple_metadata[index] = state;
        switch(state)
        {
            case 0: //no wire
                base.setWire(index, 0);
                break;
            case 1: //input
                base.setWire(index, 3);
                break;
            case 2: //output
                base.setWire(index, 9);
                break;
            default:
                break;
        }
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
    public override int spriteID { get; } = 1;
    public override Tile getTile(Vector2Int position)
    {
        Tile tile = new Tile_lamp(position, metadata);
        tile.updatePowered();
        return tile;
    }

    public override void setWire(int index, byte state)
    {
        simple_metadata[index] = state;
        switch (state)
        {
            case 0: //no wire
                base.setWire(index, 0);
                break;
            case 1: //input
                base.setWire(index, 11);
                break;
            case 2: //output
                base.setWire(index, 13);
                break;
            default:
                break;
        }
    }
}

public class UI_Tile_button : UI_Tile
{
    public override int spriteID { get; } = 2;
    public override Tile getTile(Vector2Int position)
    {
        return new Tile_button(position, metadata);
    }

    public override void setWire(int index, byte state)
    {
        simple_metadata[index] = state;
        switch (state)
        {
            case 0: //no wire
                base.setWire(index, 0);
                break;
            case 1: //output
                base.setWire(index, 9);
                break;
            default:
                break;
        }
    }
}
