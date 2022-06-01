using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_wire : Tile
{
    public override int id { get; } = 2;

    public override byte[] metadata { set; get; } = { 3, 0, 3, 0, 0};
}
