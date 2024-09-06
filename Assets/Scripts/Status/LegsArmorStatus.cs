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
    //          初期化
    //
    //
    //=======================================================
    virtual public void initialize()
    {
        Debug.Log("initialize実行");
    }
}
