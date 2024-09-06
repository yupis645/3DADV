using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LegsArmorStatus", menuName = "StatusData/LegsArmorStatus")]
public class LegsArmorStatus : ScriptableObject
{

    public int iD;
    public string Name;

    public int HP;
    public float AddJumpPower;
    public float AddSpeed;

    //=======================================================
    //          èâä˙âª
    //
    //
    //=======================================================
    virtual public void initialize()
    {
        Debug.Log("initializeé¿çs");
    }
}
