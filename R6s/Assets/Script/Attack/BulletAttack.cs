using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletAttack : ObjectAttack
{

    float time = 0;
    readonly float MAX_TIME = 4;


    GunType gunType = GunType.None;


    public BulletAttack(Vector3 angle, Vector3 StartPos, GunType gunType)
    {
        GameObject bullet = GameObject.Instantiate
            (
            AttackObjectManager.instance.GetAttackObject(AttackObjects.AttackObjectEnum.Bullet)
            );

        attackObject = bullet;

        attackObject.transform.eulerAngles = angle;
        attackObject.transform.position = StartPos;

        triggerDetector = attackObject.GetComponent<TriggerDetector>();


        SetAngle(angle);

        attackID = AttackObjectManager.instance.SetAttack(this);

        triggerDetector.SetHitAction(HitAction);
        this.gunType = gunType;

    }
    public override void HitAction(GameObject hitObject, Vector3 hitPos)
    {
        HitObject hit = HitObjectManager.instance.GetHitObject(hitObject);

        if (hit == null) return;

        Debug.Log("“–‚½‚Á‚½");

        hit.HitAction(attackID);

    }
    public override void Update()
    {
        base.Update();

        attackObject.transform.position += angle;

        time += Time.deltaTime;

        if (time > MAX_TIME) removeFlag = true;


    }

    public override void Remove()
    {
        base.Remove();

        GameObject.Destroy(attackObject);
    }

    public enum GunType 
    {
        None=-1,
        SMG,
        LMG,
        SG,
        HG,
        MP,
        MR,
        RB,
        SR,
        MAX,
        DP27=11,
        OTs03=17,
        CSRX300 =27,
    }

    #region SetGet
    public GunType GetGunType() { return gunType;}
    #endregion


}
