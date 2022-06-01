using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireMesh : MonoBehaviour
{
    private Mesh mesh;

    public List<Vector3> vertices;
    public List<int> triangles;
    List<Vector2> uvs;

    private int vertexIndex = 0;

    private void Start()
    {
        if (GetComponent<MeshFilter>().mesh == null) GetComponent<MeshFilter>().mesh = new Mesh();
        mesh = GetComponent<MeshFilter>().mesh;
    }

    public void generateMesh(Tile[,] world)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();

        for (int y = 0; y < LevelData.size; y++)
        {
            for (int x = 0; x < LevelData.size; x++)
            {
                addFace(x, y);
            }
        }

        loadMesh();
        flushTemporaryFields();

        for (int x = 0; x < LevelData.size; x++)
        {
            for (int y = 0; y < LevelData.size; y++)
            {
                setUVAt(x, y, world[x, y].id);
            }
        }
    }

    private void loadMesh()
    {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void flushTemporaryFields()
    {
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
        vertexIndex = 0;
    }

    private void addFace(int x, int y) //adds a new face on 2D plane; x and y are bottom left corner
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


        uvs.Add(Vector2.zero);
        uvs.Add(Vector2.zero);
        uvs.Add(Vector2.zero);
        uvs.Add(Vector2.zero);
    }

    public void setUVAt(int x, int y, int id) // position in bottom left corner of face
    {
        int posInArray = (x + y * LevelData.size) * 4;

        Vector2 uvPos = getUVPosition(id);

        Vector2[] mesh_uvs = mesh.uv;

        mesh_uvs[posInArray] = uvPos; // bottom left corner
        mesh_uvs[posInArray + 1] = uvPos + Vector2.right * LevelData.tileSize; // bottom right corner
        mesh_uvs[posInArray + 2] = uvPos + Vector2.up * LevelData.tileSize;    // top left corner
        mesh_uvs[posInArray + 3] = uvPos + Vector2.one * LevelData.tileSize; // top right corner

        mesh.uv = mesh_uvs;
    }

    private Vector2 getUVPosition(int id)
    {
        float x = LevelData.tileSize * (id % 8);
        float y = LevelData.tileSize * 7 - LevelData.tileSize * (id - (id % 8)) / 8;

        return new Vector2(x, y);
    }
}
