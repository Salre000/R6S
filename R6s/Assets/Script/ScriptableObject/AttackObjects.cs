using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AttackObject")]
public class AttackObjects : ScriptableObject
{
    public List<GameObject> attackObjects=new List<GameObject>();

    public enum AttackObjectEnum 
    {
        None = -1,
        Bullet,
        FinalBlow,
        Max,



    }

}
