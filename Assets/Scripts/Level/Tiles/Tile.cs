using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile
{
    public virtual byte id { get; protected set; }

    public virtual byte[] metadata { get; set; }

    protected bool powered // automatically updates visuals upon being set
    {
        get { return poweredStorage; }
        set
        {
            if(value && !powered)
            {
                id++;
                for (int i = 0; i < 4; i++)
                {
                    if (metadata[i] != 0) metadata[i]++;
                }

                BuildController.active.setTile(position, this);
            }
            else if(!value && powered)
            {
                id--;
                for(int i = 0; i < 4; i++)
                {
                    if(metadata[i] != 0) metadata[i]--;
                }

                BuildController.active.setTile(position, this);
            }

            poweredStorage = value;
        }
    }
    private bool poweredStorage = false;

    protected bool tagged = false;

    private Vector2Int position;

    protected Tile(Vector2Int position)
    {
        this.position = position;
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
                if (LogicManager.isOutputID(metadata[i]) && (!neighbour.getPowered() || neighbour is Tile_wire) && LogicManager.isInputID(neighbour.metadata[(i + 2) % 4])) // checks if this tile output at i and neighbour has input at opposite of i
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
            if (LogicManager.isInputID(metadata[i]) && neighbour.getPowered() && LogicManager.isOutputID(neighbour.metadata[(i + 2) % 4])) // checks if this tile has input at i and neighbour has output at opposite of i
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
            if (LogicManager.isOutputID(metadata[i]) && neighbour.getPowered() && LogicManager.isInputID(neighbour.metadata[(i + 2) % 4])) // checks if this tile is outputting into neighbour tiles input
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
            if (LogicManager.isInputID(metadata[i]) && neighbour.getPowered() && LogicManager.isOutputID(neighbour.metadata[(i + 2) % 4])) // checks if this tile input at i and neighbour has output at opposite of i
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
