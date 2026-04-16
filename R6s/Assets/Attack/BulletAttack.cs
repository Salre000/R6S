using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletAttack : ObjectAttack
{
    

    public BulletAttack(Vector3 angle,Vector3 StartPos) 
    {
        GameObject bullet = GameObject.Instantiate
            (
            AttackObjectManager.instance.GetAttackObject(AttackObjects.AttackObjectEnum.Bullet)
            );

        attackObject = bullet;

        attackObject.transform.eulerAngles = angle;
        attackObject.transform.position = StartPos;

        SetAngle(angle);

        AttackObjectManager.instance.SetAttack(this);

    }

    public override void UPDate()
    {

        attackObject.transform.position += angle;


    }




}
