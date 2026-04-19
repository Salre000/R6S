using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObjectManager : MonoBehaviour
{
    public static HitObjectManager instance;
    List<HitObject> hitObjects=new List<HitObject>();


    public void Awake()
    {
        instance = this;
    }
    public int SetHitObject(HitObject attack)
    {
        int ID = -1;
        for (int i = 0; i < hitObjects.Count; i++)
        {
            if (hitObjects[i] != null) continue;

            ID = i;
            hitObjects[ID] = attack;
            return ID;
        }

        hitObjects.Add(attack);

        return hitObjects.Count - 1;
    }

    public HitObject GetHitObject(int id) { return hitObjects[id]; }

    public HitObject GetHitObject(GameObject gameObject)
    {
        int ID = hitObjects.FindIndex(hit => hit.GetHitObject() == gameObject);
        if (ID == -1) return null;
        if (ID >= hitObjects.Count) return null;


        return hitObjects[ID];

    }
}
