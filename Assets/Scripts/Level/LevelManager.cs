using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public LevelMesh levelMesh;
    
    void Start()
    {
        int [,] world = new int[LevelData.size, LevelData.size];

        for(int x = 0; x < LevelData.size; x++)
        {
            for(int y = 0; y < LevelData.size; y++)
            {
                world[x, y] = 0;
            }
        }

        LevelData.world = world;

        levelMesh.generateMesh(LevelData.world);
    }
}
