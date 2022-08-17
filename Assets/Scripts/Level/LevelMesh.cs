using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMesh : MonoBehaviour
{
    private Mesh mesh;

    public UnityEngine.Rendering.IndexFormat indexFormat;

    public Vector3[] vertices;
    public int[] triangles;

    Vector2[][] uvMaps = new Vector2[5][];

    private int vertexIndex = 0;
    private int triangleIndex = 0;
    private int sizeX, sizeY;

    private bool[] uvsChanged = {false, false, false, false, false };

    private void Awake()
    {
        if (GetComponent<MeshFilter>().mesh == null) GetComponent<MeshFilter>().mesh = new Mesh();
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.indexFormat = indexFormat;
    }

    private void LateUpdate()
    {
        for(int i = 0; i < uvsChanged.Length; i++)
        {
            if(uvsChanged[i])
            {
                mesh.SetUVs(i, uvMaps[i]);
                uvsChanged[i] = false;
            }
        }
    }

    public void generateMesh(Tile[,] world)
    {
        generateMesh(world, 0);
    }

    public void generateMesh(Tile[,] world, float gapSize)
    {
        sizeX = world.GetLength(0);
        sizeY = world.GetLength(1);

        vertices = new Vector3[sizeX * sizeY * 4];
        triangles = new int[(int)(vertices.Length * 1.5f)];

        for (int i = 0; i < uvMaps.Length; i++) uvMaps[i] = new Vector2[vertices.Length];

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                addFace(x + gapSize * x, y + gapSize * y);
            }
        }

        loadMesh();
        flushTemporaryFields();

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                setUVAt(x, y, world[x, y].id);

                byte[] metadata = world[x, y].metadata;

                for (int i = 0; i < metadata.Length && i < 4; i++)
                {
                    if (metadata[i] != 0) setUVAt(i + 1, x, y, metadata[i], (Orientation)i, i > 1);
                    else setUVAt(i + 1, x, y, 64, Orientation.north, false);
                }
            }
        }
    }

    private void loadMesh()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;


        for(int i = 0; i < uvMaps.Length; i++)
        {
            mesh.SetUVs(i, uvMaps[i]);
        }

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void flushTemporaryFields()
    {
        vertices = null;
        triangles = null;
        vertexIndex = 0;
        triangleIndex = 0;
    }

    private void addFace(float x, float y) //adds a new face on 2D plane; x and y are bottom left corner
    {
        vertices[vertexIndex] = new Vector3(x, 0, y);
        vertices[vertexIndex + 1] = new Vector3(x + 1, 0, y);
        vertices[vertexIndex + 2] = new Vector3(x, 0, y + 1);
        vertices[vertexIndex + 3] = new Vector3(x + 1, 0, y + 1);
        
        triangles[triangleIndex++] = vertexIndex;
        triangles[triangleIndex++] = vertexIndex + 2;
        triangles[triangleIndex++] = vertexIndex + 1;
        triangles[triangleIndex++] = vertexIndex + 2;
        triangles[triangleIndex++] = vertexIndex + 3;
        triangles[triangleIndex++] = vertexIndex + 1;


        for(int i = 0; i < uvMaps.Length; i++)
        {
            uvMaps[i][vertexIndex] = Vector2.zero;
            uvMaps[i][vertexIndex + 1] = Vector2.zero;
            uvMaps[i][vertexIndex + 2] = Vector2.zero;
            uvMaps[i][vertexIndex + 3] = Vector2.zero;
        }

        vertexIndex += 4;
    }

    public void setUVAt(int x, int y, int id) // position in bottom left corner of face
    {
        setUVAt(0, x, y, id, Orientation.north, false);
    }

    public void setUVAt(int uvID, int x, int y, int id, Orientation orientation, bool flipped) // position in bottom left corner of face
    {

        int posInArray = (x + y * sizeX) * 4;

        Vector2 uvPos = getUVPosition(id);

        Vector2[] mesh_uvs = uvMaps[uvID];

        switch(orientation)
        {
            case Orientation.north:
                mesh_uvs[posInArray + (flipped ? 1 : 0)] = uvPos; // bottom left corner
                mesh_uvs[posInArray + (flipped ? 0 : 1)] = uvPos + Vector2.right * LevelData.tileSize; // bottom right corner
                mesh_uvs[posInArray + (flipped ? 3 : 2)] = uvPos + Vector2.up * LevelData.tileSize;    // top left corner
                mesh_uvs[posInArray + (flipped ? 2 : 3)] = uvPos + Vector2.one * LevelData.tileSize; // top right corner
                break;
            case Orientation.east:
                mesh_uvs[posInArray + (flipped ? 0 : 2)] = uvPos; // bottom left corner
                mesh_uvs[posInArray + (flipped ? 2 : 0)] = uvPos + Vector2.right * LevelData.tileSize; // bottom right corner
                mesh_uvs[posInArray + (flipped ? 1 : 3)] = uvPos + Vector2.up * LevelData.tileSize;    // top left corner
                mesh_uvs[posInArray + (flipped ? 3 : 1)] = uvPos + Vector2.one * LevelData.tileSize; // top right corner
                break;
            case Orientation.south:
                mesh_uvs[posInArray + (flipped ? 2 : 3)] = uvPos; // bottom left corner
                mesh_uvs[posInArray + (flipped ? 3 : 2)] = uvPos + Vector2.right * LevelData.tileSize; // bottom right corner
                mesh_uvs[posInArray + (flipped ? 0 : 1)] = uvPos + Vector2.up * LevelData.tileSize;    // top left corner
                mesh_uvs[posInArray + (flipped ? 1 : 0)] = uvPos + Vector2.one * LevelData.tileSize; // top right corner
                break;
            case Orientation.west:
                mesh_uvs[posInArray + (flipped ? 3 : 1)] = uvPos; // bottom left corner
                mesh_uvs[posInArray + (flipped ? 1 : 3)] = uvPos + Vector2.right * LevelData.tileSize; // bottom right corner
                mesh_uvs[posInArray + (flipped ? 2 : 0)] = uvPos + Vector2.up * LevelData.tileSize;    // top left corner
                mesh_uvs[posInArray + (flipped ? 0 : 2)] = uvPos + Vector2.one * LevelData.tileSize; // top right corner
                break;
        }

        uvsChanged[uvID] = true;
    }

    private Vector2 getUVPosition(int id)
    {
        float x = LevelData.tileSize * (id % 8);
        float y = LevelData.tileSize * 7 - LevelData.tileSize * (id - (id % 8)) / 8;

        return new Vector2(x, y);
    }
}


public enum Orientation
{
    north, east, south, west
}