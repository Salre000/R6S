using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attackクラスによって攻撃が当たるクラス
/// </summary>
public class HitObject
{
    GameObject hitObject;

    public virtual void HitAction(int attackID) 
    {


        


    }

    #region SetGet

    public void SetHitObject(GameObject game) {hitObject = game;}
    public GameObject GetHitObject() {return hitObject;}

    #endregion
}
