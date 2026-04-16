using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectManager : MonoBehaviour
{
    [SerializeField] AttackObjects attackObjects;

    public static AttackObjectManager instance;

    List<Attack> attacks = new List<Attack>();


    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        AttackUPDate();
    }


    public void AttackUPDate() 
    {
        if (attacks.Count < 1) return;
        for(int i=0;i< attacks.Count;i++) attacks[i].UPDate();

    }

    public GameObject GetAttackObject(AttackObjects.AttackObjectEnum objectEnum)
    {
        return attackObjects.attackObjects[(int)objectEnum];
    }

    public void SetAttack(Attack attack) 
    {
        attacks.Add(attack);
    }


}
