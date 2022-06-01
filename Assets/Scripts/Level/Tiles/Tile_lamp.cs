using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_lamp : Tile
{
    public override int id { get; } = 5;

    public override byte[] metadata { set; get; } = {11, 13, 11, 13 };
}
