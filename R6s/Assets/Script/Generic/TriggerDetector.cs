using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クラス同士の依存を避けるための緩衝クラス
/// オブジェクトが当たったときのクラス
/// TriggerON
/// </summary>
public class TriggerDetector : MonoBehaviour
{

    private System.Action<GameObject, Vector3> hitAction;

    public void OnTriggerEnter(Collider other)
    {
        if (hitAction == null) return;

        // 当たった位置からこのオブジェクトに一番近い位置(妥協)
        Vector3 hitPoint = other.ClosestPoint(transform.position);

        hitAction(other.gameObject, hitPoint);


    }



    public void SetHitAction(System.Action<GameObject, Vector3> action) { hitAction=action; }
}
