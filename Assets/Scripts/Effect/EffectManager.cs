using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effect;

public static class EffectManager
{

    public static void ChangeColor(GameObject go, Collision co, Material target, float time = 1)
    {
        ChangeMatColor cc = go.AddComponent<ChangeMatColor>();
        cc.contact = co.contacts[0].point;
        cc.target = target;
        cc.time = time;
    }

}
