using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_lamp : Tile
{
    public override byte id { get; protected set; } = 5;
    public override byte simpleID { get; } = 1; //same as ui_tile's spriteID
    public override byte[] ioIDs { get; } = { 11, 13 };

    public Tile_lamp(Vector2Int position) : base(position) { }
    public Tile_lamp(Vector2Int position, byte[] metadata) : base(position, metadata) { }
}
