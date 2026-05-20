using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UŒ‚‚ÉŠÖŒW‚·‚éÅ‚à‰º‚ÌƒNƒ‰ƒX
/// </summary>

public class Attack
{

    /// <summary>
    /// ‚±‚ÌUŒ‚©‘Ì‚ÌID
    /// </summary>
    protected int attackID = -1;

    /// <summary>
    /// UŒ‚‚Ì‘®«‚ÌID
    /// </summary>
    protected AttackAttribute.attackAttribute  attributeID = AttackAttribute.attackAttribute.None;

    /// <summary>
    /// UŒ‚‚Ég—p‚·‚éUŒ‚—Í
    /// </summary>
    protected int power = -1;

    /// <summary>
    /// UŒ‚‚Ì”ÍˆÍ‚Ì’l
    /// </summary>
    protected float range = 0;

    /// <summary>
    /// UŒ‚‚Ìày—ô‚·‚éÀ•W
    /// </summary>
    protected Vector3 hitPos = Vector3.zero;

    /// <summary>
    /// UŒ‚‚ÌŒü‚«@•K—v‚ª‚È‚¢ê‡‚Í‚O‚Ì‚Ü‚Ü‚É‚·‚é
    /// </summary>
    protected Vector3 angle = Vector3.zero;

    /// <summary>
    /// ‚±‚Ìƒtƒ‰ƒO‚ªtrue‚É‚È‚Á‚½‚çƒŠƒ€[ƒu‚ğ‹N“®‚·‚é
    /// </summary>
    protected bool removeFlag = false;  


    public virtual void Update() { if (removeFlag) Remove(); }

    public virtual void Remove()
    {
        AttackObjectManager.instance.RemoveAttack(attackID);
    }


    public virtual void HitAction(GameObject hitObject,Vector3 hitPos) 
    {
        HitObject hit = HitObjectManager.instance.GetHitObject(hitObject);

        if (hit == null) return;

        SetHitPos(hitPos);

        hit.HitAction(attackID);
    } 


    #region SetGet


    public void SetAttackID(int value) { attackID = value; }
    public int GetAttackID() { return attackID; }
    public void SetAttributeID(AttackAttribute.attackAttribute value) { attributeID = value; }
    public AttackAttribute.attackAttribute GetAttributeID() { return attributeID; }
    public void SetPower(int value) { power = value; }
    public int GetPower() { return power; }
    public void SetRanger(float value) { range = value; }
    public float GetRanger() { return range; }
    public void SetHitPos(Vector3 value) { hitPos = value; }
    public Vector3 GetHitPos() { return hitPos; }
    public void SetAngle(Vector3 value) { angle = value; }
    public Vector3 GetAngle() { return angle; }

    #endregion


}
