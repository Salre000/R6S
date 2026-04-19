using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject barricade;


    // Start is called before the first frame update
    void Start()
    {
        CreateBariicade();

        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) Shot();

       

    }


    private void Shot() 
    {
        BulletAttack bulletAttack = new BulletAttack(Camera.main.transform.forward/100f, Camera.main.transform.position);

    }

    private void CreateBariicade() 
    {
        Barricade barricade = new Barricade(this.barricade);
    }


}
