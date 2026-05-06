using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attackクラスによって攻撃が当たるクラス
/// </summary>
public class HitObject
{
    GameObject hitObject;

    int hitID = -1;
    /// <summary>
    /// このフラグがtrueになったらリムーブを起動する
    /// </summary>
    protected bool removeFlag = false;


    public virtual void HitAction(int attackID) 
    {


        


    }


    public virtual void Update() { if (removeFlag) Remove(); }

    public virtual void Remove()
    {
        HitObjectManager.instance.RemoveHitObject(hitID);
    }


    #region SetGet

    public void SetHitObject(GameObject game) {hitObject = game;}
    public GameObject GetHitObject() {return hitObject;}
    public void SetHitID(int id) {hitID = id;}
    public int GetHitID() {return hitID;}

    #endregion
}
