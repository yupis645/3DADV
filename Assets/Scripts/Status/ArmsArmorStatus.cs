using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "ArmsarmorStatus", menuName = "StatusData/ArmsarmorStatus")]
public class ArmsArmorStatus : ScriptableObject
{

    public int ID;
    public string Name;

    public int HP;
    public float FireArmMagnif;
    public float MeleeWeaponMagnif;

    //=======================================================
    //          初期化
    //
    //
    //=======================================================
    virtual public void Initialize()
    {
        Debug.Log("initialize実行");
    }

}
