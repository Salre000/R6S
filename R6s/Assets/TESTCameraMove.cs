using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTCameraMove : MonoBehaviour
{
    private Vector3 mousePos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        mousePos = Input.mousePosition;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector3 = Input.mousePosition;

        Vector3 vec = mousePos-vector3;

        Vector3 angle = transform.eulerAngles;

        angle.x += vec.y / 20f;
        angle.y -= vec.x/20f;

        transform.eulerAngles = angle;

        mousePos = Input.mousePosition;

    }


}
