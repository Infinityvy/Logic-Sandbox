using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_wire : Tile
{
    public override byte id { get; protected set; } = 1;

    public override byte[] metadata { set; get; } = { 3, 0, 9, 0, 0};

    public Tile_wire(Vector2Int position) : base(position) { }
}
