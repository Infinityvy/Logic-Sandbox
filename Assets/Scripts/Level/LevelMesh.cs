using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMesh : MonoBehaviour
{
    private Mesh mesh;

    public UnityEngine.Rendering.IndexFormat indexFormat;

    public List<Vector3> vertices;
    public List<int> triangles;

    List<Vector2>[] uvs = new List<Vector2>[5];

    private int vertexIndex = 0;
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
                mesh.SetUVs(i, uvs[i]);
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

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int i = 0; i < uvs.Length; i++) uvs[i] = new List<Vector2>();

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                addFace(x + gapSize, y + gapSize);
            }
        }

        loadMesh();
        Debug.Log(vertices.Count);
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
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();


        for(int i = 0; i < uvs.Length; i++)
        {
            mesh.SetUVs(i, uvs[i]);
        }

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void flushTemporaryFields()
    {
        vertices.Clear();
        triangles.Clear();
        vertexIndex = 0;
    }

    private void addFace(float x, float y) //adds a new face on 2D plane; x and y are bottom left corner
    {
        vertices.Add(new Vector3(x, 0, y));
        vertices.Add(new Vector3(x + 1, 0, y));
        vertices.Add(new Vector3(x, 0, y + 1));
        vertices.Add(new Vector3(x + 1, 0, y + 1));
        
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
        triangles.Add(vertexIndex + 1);

        vertexIndex += 4;


        for(int i = 0; i < uvs.Length; i++)
        {
            uvs[i].Add(Vector2.zero);
            uvs[i].Add(Vector2.zero);
            uvs[i].Add(Vector2.zero);
            uvs[i].Add(Vector2.zero);
        }
    }

    public void setUVAt(int x, int y, int id) // position in bottom left corner of face
    {
        setUVAt(0, x, y, id, Orientation.north, false);
    }

    public void setUVAt(int uvID, int x, int y, int id, Orientation orientation, bool flipped) // position in bottom left corner of face
    {

        int posInArray = (x + y * sizeX) * 4;

        Vector2 uvPos = getUVPosition(id);

        List<Vector2> mesh_uvs = uvs[uvID];

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