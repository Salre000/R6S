using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTPROGRM : MonoBehaviour
{
    public int chunkSize = 10;

    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        Dictionary<Vector2Int, List<int>> chunkTriangles =
            new Dictionary<Vector2Int, List<int>>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 center =
                (vertices[triangles[i]] +
                 vertices[triangles[i + 1]] +
                 vertices[triangles[i + 2]]) / 3f;

            Vector2Int chunk =
                new Vector2Int(
                    Mathf.FloorToInt(center.x / chunkSize),
                    Mathf.FloorToInt(center.z / chunkSize)
                );

            if (!chunkTriangles.ContainsKey(chunk))
                chunkTriangles[chunk] = new List<int>();

            chunkTriangles[chunk].Add(triangles[i]);
            chunkTriangles[chunk].Add(triangles[i + 1]);
            chunkTriangles[chunk].Add(triangles[i + 2]);
        }

        foreach (var pair in chunkTriangles)
        {
            CreateChunkMesh(pair.Key, pair.Value, vertices);
        }
    }

    void CreateChunkMesh(
        Vector2Int coord,
        List<int> tris,
        Vector3[] originalVertices)
    {
        Mesh chunkMesh = new Mesh();

        chunkMesh.vertices = originalVertices;
        chunkMesh.triangles = tris.ToArray();

        GameObject obj = new GameObject($"Chunk_{coord}");

        obj.AddComponent<MeshFilter>().mesh = chunkMesh;
        obj.AddComponent<MeshRenderer>().material =
            GetComponent<MeshRenderer>().material;
    }
}
