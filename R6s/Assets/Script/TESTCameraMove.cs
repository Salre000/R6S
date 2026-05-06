using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraMove : MonoBehaviour
{
    private Vector3 mousePos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        mousePos = Input.mousePosition;
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        AngleChange();
        Move();
        MouseLimit();
    }

    private void AngleChange()
    {
        Vector3 vector3 = Input.mousePosition;

        Vector3 vec = mousePos - vector3;

        Vector3 angle = transform.eulerAngles;

        angle.x += vec.y / 20f;
        angle.y -= vec.x / 20f;

        transform.eulerAngles = angle;

        mousePos = Input.mousePosition;


    }


    private void Move()
    {
        Vector3 vector = transform.position;

        if (Input.GetKey(KeyCode.W)) vector += transform.forward / 30;
        if (Input.GetKey(KeyCode.A)) vector -= transform.right / 30;
        if (Input.GetKey(KeyCode.S)) vector -= transform.forward / 30;
        if (Input.GetKey(KeyCode.D)) vector += transform.right / 30;

        if (Input.GetKey(KeyCode.Space)) vector.y += 1f / 30;
        if (Input.GetKey(KeyCode.LeftControl)) vector.y -= 1f / 30;
        if (Input.GetKey(KeyCode.L)) Cursor.lockState = CursorLockMode.None;




        transform.position = vector;
    }


    private void MouseLimit()
    {


    }

}
