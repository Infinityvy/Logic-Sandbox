using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_wire : Tile
{
    public override byte id { get; protected set; } = 1;
    public override byte simpleID { get; } = 0; //same as ui_tile's spriteID
    public override byte[] ioIDs { get; } = { 3, 9 };


    public Tile_wire(Vector2Int position) : base(position) { }
    public Tile_wire(Vector2Int position, byte[] metadata) : base(position, metadata) { }

    public override void setPowered(bool state)
    {
        LogicManager.active.StartCoroutine(delaySetPowered(state));
    }

    public override bool isConnectedToPowerSource()
    {
        LogicManager.active.StartCoroutine(delayIsConnectedToPowerSource());
        return powered;
    }

    private IEnumerator delaySetPowered(bool state)
    {
        byte delay = metadata[4];
        yield return new WaitForSeconds(delay * 0.05f);

        if (powered != state)
        {
            base.setPowered(state);
        }
    }

    private IEnumerator delayIsConnectedToPowerSource()
    {
        byte delay = metadata[4];
        yield return new WaitForSeconds(delay * 0.05f);

        base.isConnectedToPowerSource();
    }
}
