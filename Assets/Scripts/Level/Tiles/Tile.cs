using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile
{
    public virtual int id { get; }

    public virtual byte[] metadata { get; set; }

    private bool powered = false;



    public void setPowered(bool state)
    {
        powered = state;
    }
}
