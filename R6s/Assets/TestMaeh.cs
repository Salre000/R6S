using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMaeh : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

        Mesh mesh = new Mesh();

        Vector3[] vector3s =
            {
            new Vector3(3,3,0),
            new Vector3(3,0,0),
            new Vector3(0,3,0),
            new Vector3(0,0,0),
            new Vector3(0,0,9),

            };
        mesh.vertices = vector3s;


        int[] triangles =
        {
            0, 1, 2,
            2, 1, 3
        };

        mesh.triangles = triangles;
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        gameObject.AddComponent<MeshRenderer>();


    }

    // Update is called once per frame
    void Update()
    {



    }
}
