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

    private List<int> attackIds= new List<int>();
    private int hp = 100;

    private readonly int MAX_HP = 100;

    private int hitCount = 0;

    private readonly int MAX_HIT_COUNT = 20;

    private readonly int MAX_HIDE_PARTS = 14;

    private readonly int PARTS_WIDE=3;
    private readonly int PARTS_HEIGHT =14;

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
            BoxCollider box = barricadeParts[i].AddComponent<BoxCollider>();
            box.isTrigger = true;
            barricadeParts[i].AddComponent<TriggerDetector>().SetHitAction
                ((gameObject, Pos) => 
                {
                    HitObjectAction(gameObject,cash);
                });
        }

        int id=HitObjectManager.instance.SetHitObject(this);

        SetHitID(id);

    }
    public override void HitAction(int attackID)
    {

        Attack attack =AttackObjectManager.instance.GetAttack(attackID);

        // 爆発属性以外を無視する
        if (attack.GetAttributeID() != AttackAttribute.attackAttribute.explosion) return;

        Break();

    }


    public override void Update()
    {
        base.Update();
        CheckUniqueAttackID();

    }

    private void HitObjectAction(GameObject gameObject,int cash) 
    {
        Attack attack = AttackObjectManager.instance.GetAttack(gameObject);

        
        if (attack == null) return;

        CheckFinalBlow(attack, barricadeParts[cash]);

        BulletAttack bulletAttack = attack as BulletAttack;

        if (bulletAttack == null) return;


        if (hitBarricadeParts.Contains(cash) || 4 < (int)bulletAttack.GetGunType())
        { 
            barricadeParts[cash].SetActive(false);
            CheckSupport();
        }

        hitBarricadeParts.Add(cash);

        CheckHp(bulletAttack.GetGunType());

        CheckPartsActive();
    }

    /// <summary>
    ///  バリケードのHpを確認する関数
    /// </summary>
    private void CheckHp(BulletAttack.GunType gunType) 
    {
        hitCount++;
        if(hitCount>=MAX_HIT_COUNT) { Break();return;}

        hp -= GunTypeDamage(gunType);

        if (hp <= 0) Break();

    }

    private int GunTypeDamage(BulletAttack.GunType gunType) 
    {
        int Damage = 0;

        switch (gunType)
        {
            case BulletAttack.GunType.SMG:
                break;
            case BulletAttack.GunType.DP27:
            case BulletAttack.GunType.LMG:
                break;
            case BulletAttack.GunType.SG:
                break;
            case BulletAttack.GunType.HG:
                break;
            case BulletAttack.GunType.MP:
                break;
            case BulletAttack.GunType.MR:
                Damage = 15;
                break;
            case BulletAttack.GunType.RB:
                break;
            case BulletAttack.GunType.SR:
                Damage = 34;
                break;
            case BulletAttack.GunType.OTs03:
                Damage = 34;
                break;
            case BulletAttack.GunType.CSRX300:
                Damage = MAX_HP;
                break;
        }

        return Damage;

    }

    private void CheckPartsActive() 
    {
        int hideObject = 0;

        for (int i=0;i< barricadeParts.Count; i++) 
        {
            if (barricadeParts[i].activeSelf) continue;

            hideObject++;


        }

        if(hideObject>=MAX_HIDE_PARTS) Break();
    }

    private void CheckSupport() 
    {
        for (int i = 0; i < barricadeParts.Count; i++)
        {
            //バリケードの中央のパーツ以外を無視
            if ((i + 1) % PARTS_WIDE != 2) continue;

            int support = 4;

            if (barricadeParts[i - 1].activeSelf == false &&
                barricadeParts[i + 1].activeSelf == false)
                support -= 2;

            if (i < PARTS_WIDE || !barricadeParts[i - PARTS_WIDE].activeSelf)
                support--;
            if (i > barricadeParts.Count-PARTS_WIDE || !barricadeParts[i + PARTS_WIDE].activeSelf)
                support--;

            if (support > 0) continue;

            barricadeParts[i].SetActive(false);

        }





    }

    private void CheckFinalBlow(Attack attack,GameObject partsObject) 
    {
        FinalBlow finalBlow = attack as FinalBlow;

        if (finalBlow == null) return;

        partsObject.SetActive(false);

        attackIds.Add(attack.GetAttackID());

    }

    private void CheckUniqueAttackID()
    {
        if (attackIds.Count == 0) return;

        int count = 0;
        List<int> duplicate=new List<int>();
        for (int i = 0; i < attackIds.Count; i++) 
        {
            if (duplicate.Contains(i)) continue;

            for(int j=0;j< attackIds.Count; j++) 
            {
                if (i == j) continue;
                if (attackIds[i] != attackIds[j]) continue;

                duplicate.Add(j);

            }

            count++;
        }

        attackIds.Clear();

        hitCount -= count;

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

        hp = MAX_HP;
        hitCount = 0;

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
