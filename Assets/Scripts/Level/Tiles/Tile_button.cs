using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_button : Tile
{
    public override int id { get; } = 7;


    public override byte[] metadata { set; get; } = { 0, 0, 0, 9 };
}
