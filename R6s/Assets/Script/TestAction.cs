using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestAction : MonoBehaviour
{
    public GameObject barricade;

    public BulletAttack.GunType gunType = BulletAttack.GunType.CSRX300;

    public TextMeshProUGUI textMeshProUGUI;

    System.Action action;

    List<string> gunName = new List<string>()
    {
        "SMG",
        "LMG",
        "SG",
        "HG",
        "MP",
        "MR",
        "RB",
        "SR",
        "OTS03",
        "CSRX300"
};


    // Start is called before the first frame update
    void Start()
    {
        CreateBarricade();

        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) Shot();
        if (Input.GetMouseButtonDown(1)) FinalBlow();
        if (Input.GetKeyDown(KeyCode.R)) action();

        int ID = (int)gunType;

        if (ID > 20) ID = 9;
        if (ID > 10) ID = 8;

        textMeshProUGUI.text = gunName[ID];


        GunChange();
    }


    private void Shot()
    {
        BulletAttack bulletAttack = new BulletAttack(Camera.main.transform.forward / 50f, Camera.main.transform.position, gunType);
    }

    private void FinalBlow()
    {
        // カメラからクリック位置に向かうRayを生成
        RaycastHit hit;

        // Rayを投射し、オブジェクトに当たった場合
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 30, out hit))
        {
            FinalBlow final = new FinalBlow(hit.point, 0.3f);
        }

    }

    private void CreateBarricade()
    {
        Barricade barricade = new Barricade(this.barricade);

        action = barricade.Reboot;
    }

    private void GunChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) gunType = (BulletAttack.GunType)1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) gunType = (BulletAttack.GunType)2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) gunType = (BulletAttack.GunType)3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) gunType = (BulletAttack.GunType)4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) gunType = (BulletAttack.GunType)5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) gunType = (BulletAttack.GunType)6;
        if (Input.GetKeyDown(KeyCode.Alpha7)) gunType = (BulletAttack.GunType)7;
        if (Input.GetKeyDown(KeyCode.Alpha8)) gunType = (BulletAttack.GunType)17;
        if (Input.GetKeyDown(KeyCode.Alpha9)) gunType = (BulletAttack.GunType)27;


    }


}
