using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_lamp : Tile
{
    public override byte id { get; protected set; } = 5;

    public override byte[] metadata { set; get; } = {11, 13, 13, 11 };

    public Tile_lamp(Vector2Int position) : base(position) { }
}
