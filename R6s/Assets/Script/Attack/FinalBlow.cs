using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BulletAttack;

public class FinalBlow : ObjectAttack
{
    private float time = 0;
    private readonly float MaxTime = 1;

    private readonly float range = 3; 
    public FinalBlow(Vector3 pos,float range) 
    {
        GameObject bullet = GameObject.Instantiate
    (
    AttackObjectManager.instance.GetAttackObject(AttackObjects.AttackObjectEnum.FinalBlow)
    );

        bullet.transform.position = pos;
        bullet.transform.localScale = new Vector3(range, range, range);


        triggerDetector=bullet.AddComponent<TriggerDetector>();

        attackObject = bullet;


        attackID = AttackObjectManager.instance.SetAttack(this);

        triggerDetector.SetHitAction(HitAction);



    }



    public override void HitAction(GameObject hitObject, Vector3 hitPos)
    {
        //‹ó”’

        Debug.Log("Hit");

    }
    public override void Update()
    {
        base.Update();

        time += Time.deltaTime;
        if (time < MaxTime) return;

        removeFlag = true;

    }

    public override void Remove()
    {
        base.Remove();

        GameObject.Destroy(attackObject);
    }

}
