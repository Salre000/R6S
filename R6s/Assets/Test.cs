using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject Gun;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!Input.GetMouseButtonDown(0)) return;

        Shot();

    }


    private void Shot() 
    {
        BulletAttack bulletAttack = new BulletAttack(Gun.transform.forward/100f, Gun.transform.position);

        Debug.Log(Gun.transform.position);
    }


}
