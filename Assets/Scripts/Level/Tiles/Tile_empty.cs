using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_empty : Tile
{
    public override int id { get; } = 0;

    public override byte[] metadata { set; get; } = {0, 0, 0, 0 };
}
