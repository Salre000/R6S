using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 判定以外にもオブジェクトを持つ攻撃
/// </summary>
public class ObjectAttack : Attack
{
    protected GameObject attackObject;


    public void SetAttackObject(GameObject gameObject) {attackObject = gameObject;}
    public GameObject GetAttackObject() { return attackObject;}



}
