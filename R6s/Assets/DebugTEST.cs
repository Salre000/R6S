using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTEST : MonoBehaviour
{
    public static DebugTEST Instance;

    public GameObject debugObject;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }


    public void ShowDebugObject(Vector3 pos,string name="Debug") 
    {
        GameObject gameObject=Instantiate(debugObject);

        gameObject.transform.position = pos;

        gameObject.transform.name = name;


    }
}
