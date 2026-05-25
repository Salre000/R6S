using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTEST : MonoBehaviour
{
    public static DebugTEST Instance;

    public List<GameObject> gameObjects;
    public List<GameObject> gameMesh;
    public GameObject debugObject;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }


    public void ShowDebugObject(Vector3 pos, string name = "Debug")
    {
        GameObject gameObject = Instantiate(debugObject);

        gameObject.transform.position = pos;

        gameObject.transform.name = name;

        gameObjects.Add(gameObject);


    }

    public void Lsot() { foreach (GameObject gameObject in gameObjects) Destroy(gameObject); }

    public void ShowMesh(GameObject wallObject,int i,int j,int p)
    {
        GameObject wall =Instantiate(debugObject);

        MeshFilter meshFilter = wall.GetComponent<MeshFilter>();

        List<Vector3> vector3s = new List<Vector3>();
        vector3s.Add(meshFilter.mesh.vertices[i]);
        vector3s.Add(meshFilter.mesh.vertices[j]);
        vector3s.Add(meshFilter.mesh.vertices[p]);

        List<int> indexs = new List<int>();
        indexs.Add(0);
        indexs.Add(1);
        indexs.Add(2);
        indexs.Add(0);
        indexs.Add(2);
        indexs.Add(1);

        meshFilter.mesh.vertices = vector3s.ToArray();
        meshFilter.mesh.triangles = indexs.ToArray();


    }

    public void LostMesh()
    {

        for (int i = 0; i < gameMesh.Count;i++)
        {

            Destroy(gameMesh[i]);

        }
    }

}
