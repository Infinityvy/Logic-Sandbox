using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_empty : Tile
{
    public override byte id { get; protected set; } = 0;

    public override byte[] metadata { set; get; } = {0, 0, 0, 0 };

    public Tile_empty(Vector2Int position) : base(position) { }

    public Tile_empty(Vector2Int position, byte[] old_metadata) : base(position)
    {
        LogicManager.active.StartCoroutine(updateOutputToNeighbours(old_metadata));
    }


    public override bool isConnectedToPowerSource()
    {
        return false;
    }

    private IEnumerator updateOutputToNeighbours(byte[] old_metadata)
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < 4; i++)
        {
            Tile neighbour = getNeighbourByIndex(i);
            if (neighbour == null) continue;
            if (LogicManager.isOutputID(old_metadata[i]) && neighbour.getPowered() && LogicManager.isInputID(neighbour.metadata[(i + 2) % 4]))
            {
                neighbour.isConnectedToPowerSource();
            }
        }
    }
}
