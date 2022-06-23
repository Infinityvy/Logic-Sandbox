using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_wire : Tile
{
    public override byte id { get; protected set; } = 1;

    public override byte[] metadata { set; get; } = { 3, 0, 9, 0, 54};

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
        for (byte delay = metadata[4]; delay != 0; delay--)
        {
            yield return new WaitForFixedUpdate();
        }

        if(powered != state)
        {
            base.setPowered(state);
        }
    }

    private IEnumerator delayIsConnectedToPowerSource()
    {
        for (byte delay = metadata[4]; delay != 0; delay--)
        {
            yield return new WaitForFixedUpdate();
        }

        base.isConnectedToPowerSource();
    }
}
