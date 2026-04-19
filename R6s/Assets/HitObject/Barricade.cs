using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Barricade : HitObject
{

    // 当たる可能性のあるオブジェクトを
    private List<GameObject> barricadeParts = new List<GameObject>();

    private readonly string hitObjectTag = "HItObject";

    private List<int> hitBarricadeParts= new List<int>();


    public Barricade(GameObject barricade) 
    {

        Rigidbody rigidbody = barricade.AddComponent<Rigidbody>();

        rigidbody.useGravity = false;

        for (int i=0;i< barricade.transform.childCount;i++) 
        {
            if (barricade.transform.GetChild(i).tag != hitObjectTag) continue;

            barricadeParts.Add(barricade.transform.GetChild(i).gameObject);
        }

        for(int i=0;i< barricadeParts.Count; i++) 
        {
            int cash = i;
            barricadeParts[i].AddComponent<BoxCollider>().isTrigger=false;
            barricadeParts[i].AddComponent<TriggerDetector>().SetHitAction
                ((gameObject, Pos) => 
                {
                    HitObjectAction(gameObject,cash);
                });
        }

    }
    public override void HitAction(int attackID)
    {

        Attack attack =AttackObjectManager.instance.GetAttack(attackID);

        // 爆発属性以外を無視する
        if (attack.GetAttributeID() != AttackAttribute.attackAttribute.explosion) return;

        Break();

    }

    private void HitObjectAction(GameObject gameObject,int cash) 
    {
        Attack attack = AttackObjectManager.instance.GetAttack(gameObject);

        if (attack == null) return;

        BulletAttack bulletAttack = attack as BulletAttack;


        if (hitBarricadeParts.Contains(cash)||4 < (int)bulletAttack.GetGunType()) barricadeParts[cash].SetActive(false);
        hitBarricadeParts.Add(cash);

        if (20 < (int)bulletAttack.GetGunType()) Break();
    }

    /// <summary>
    /// バリケードの復活関数
    /// </summary>
    public void Reboot() 
    {
        for(int i=0;i< barricadeParts.Count; i++)
        {
            barricadeParts[i].SetActive(true);
        }
        hitBarricadeParts.Clear();
    }
    public void Break() 
    {
        for(int i=0;i< barricadeParts.Count; i++)
        {
            barricadeParts[i].SetActive(false);
        }
        hitBarricadeParts.Clear();
    }



}
