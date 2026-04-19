using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectManager : MonoBehaviour
{
    [SerializeField] AttackObjects attackObjects;

    public static AttackObjectManager instance;

    List<Attack> attacks = new List<Attack>(128);


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
        for(int i=0;i< attacks.Count;i++)
            if (attacks[i]!=null)attacks[i].Update();

    }

    public GameObject GetAttackObject(AttackObjects.AttackObjectEnum objectEnum)
    {
        return attackObjects.attackObjects[(int)objectEnum];
    }


    public int SetAttack(Attack attack) 
    {
        int ID = -1;
        for(int i=0; i < attacks.Count; i++) 
        {
            if (attacks[i] != null) continue;

            ID= i;
            attacks[ID] = attack;
            return ID;
        }

        attacks.Add(attack);

        return attacks.Count-1;
    }

    public Attack GetAttack(int id) { return attacks[id]; }
    public Attack GetAttack(GameObject gameObject)
    {
        List<ObjectAttack> attackObjectList= new List<ObjectAttack>();  
        for(int i=0;i< attacks.Count; i++) 
        {
            ObjectAttack objectAttack = attacks[i] as ObjectAttack;

            if (objectAttack == null) continue;

            attackObjectList.Add(objectAttack);
        }
        int ID = attackObjectList.FindIndex(attackObject => attackObject.GetAttackObject() == gameObject);

        return attacks[ID];

    }

    public void RemoveAttack(int AttackID) { attacks[AttackID]=null; }


}
