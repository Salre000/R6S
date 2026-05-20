using UnityEngine;


public class TestCameraMove : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        AngleChange();
        Move();
    }

    private void AngleChange()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 angle = transform.eulerAngles;

        angle.x -=mouseY;
        angle.y += mouseX;

        transform.eulerAngles = angle;

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
        if (Input.GetKey(KeyCode.L)) UnityEngine.Cursor.lockState = CursorLockMode.None;




        transform.position = vector;
    }

}
