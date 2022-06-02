using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager active;

    public LevelMesh levelMesh;

    private void Awake()
    {
        active = this;
    }

    void Start()
    {
        Tile[,] world = new Tile[LevelData.size, LevelData.size];

        for(int x = 0; x < LevelData.size; x++)
        {
            for(int y = 0; y < LevelData.size; y++)
            {
                world[x, y] = new Tile_empty(new Vector2Int(x, y));
            }
        }

        LevelData.world = world;

        levelMesh.generateMesh(LevelData.world);
    }
}
