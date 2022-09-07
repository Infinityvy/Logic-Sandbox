using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile
{
    public virtual byte id { get; protected set; } = 64;
    public virtual byte simpleID { get; } = 255; //same as ui_tile's spriteID

    public virtual byte[] metadata { get; set; } = { 0, 0, 0, 0}; //0: nothing; 1: input; 2: output
    public virtual byte[] ioIDs { get; } //input and output sprite IDs in that order (length of 2)

    protected bool powered // automatically updates visuals upon being set
    {
        get { return poweredStorage; }
        set
        {
            if(value && !powered)
            {
                id++;
            }
            else if(!value && powered)
            {
                id--;
            }

            poweredStorage = value;
            BuildController.active.setTile(position, this);
        }
    }
    private bool poweredStorage = false;

    protected bool tagged = false;

    private Vector2Int position;

    protected Tile(Vector2Int position)
    {
        this.position = position;
    }

    protected Tile(Vector2Int position, byte[] metadata)
    {
        this.position = position;
        this.metadata = (byte[])metadata.Clone();
    }


    public bool getPowered()
    {
        return powered;
    }

    public virtual void setPowered(bool state)
    {
        if(state)
        {
            powered = true;

            for (int i = 0; i < 4; i++)
            {
                Tile neighbour = getNeighbourByIndex(i);
                if (neighbour == null) continue;
                if (metadata[i] == 2 && (!neighbour.getPowered() || neighbour is Tile_wire) && neighbour.metadata[(i + 2) % 4] == 1) // checks if this tile has output at i and neighbour has input at opposite of i
                {
                    neighbour.setPowered(true);
                }
            }
        }
        else
        {
            isConnectedToPowerSource();
        }
    }

    public virtual bool isConnectedToPowerSource()
    {
        tagSelf();

        for(int i = 0; i < 4; i++)
        {
            Tile neighbour = getNeighbourByIndex(i);
            if (neighbour == null) continue;
            if (metadata[i] == 1 && neighbour.getPowered() && neighbour.metadata[(i + 2) % 4] == 2) // checks if this tile has input at i and neighbour has output at opposite of i
            {
                if (neighbour.tagged) powered = true;

                if (neighbour.isConnectedToPowerSource())
                {
                    powered = true;
                    return true;
                }
            }
        }

        powered = false;

        for(int i = 0; i < 4; i++)
        {
            Tile neighbour = getNeighbourByIndex(i);
            if (neighbour == null) continue;
            if (metadata[i] == 2 && (neighbour.getPowered() || neighbour is Tile_wire) && neighbour.metadata[(i + 2) % 4] == 1) // checks if this tile is outputting into neighbour tiles input
            {
                neighbour.isConnectedToPowerSource();
            }
        }

        return false;
    }

    public void updatePowered()
    {
        for (int i = 0; i < 4; i++)
        {
            Tile neighbour = getNeighbourByIndex(i);
            if (neighbour == null) continue;
            if (metadata[i] == 1 && neighbour.getPowered() && neighbour.metadata[(i + 2) % 4] == 2) // checks if this tile input at i and neighbour has output at opposite of i
            {
                setPowered(true);
            }
        }
    }

    protected Tile getNeighbourByIndex(int index)
    {
        int x = position.x;
        int y = position.y;


        switch(index)
        {
            case 0:
                if (y + 1 > LevelData.size - 1) return null;
                return LevelData.world[x, y + 1];
            case 1:
                if (x + 1 > LevelData.size - 1) return null;
                return LevelData.world[x + 1, y];
            case 2:
                if (y - 1 < 0) return null;
                return LevelData.world[x, y - 1];
            case 3:
                if (x - 1 < 0) return null;
                return LevelData.world[x - 1, y];
            default:
                return null;
        }
    }

    protected void tagSelf()
    {
        tagged = true;
        LogicManager.active.StartCoroutine(resetTagged());
    }

    protected IEnumerator resetTagged()
    {
        yield return new WaitForEndOfFrame();
        tagged = false;
    }

    public virtual void interact() { }
}
