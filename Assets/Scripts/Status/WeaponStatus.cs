using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponStatus", menuName = "StatusData/WeaponStatus")]
public class WeaponStatus : ScriptableObject
{

    [Header("ID / 名前")]
    public int WeaponID;
    public string WeaponName;

    enum weaponCategory
    {
        firearms , boms , missile , cannon , melee , assist     //銃器,ボム,ミサイル,キャノン砲,近接武器,アシスト
    }
    [SerializeField] weaponCategory weaponType;

    [Header("ステータスパラメーター")]
    public float  AttackPower;            //攻撃力
    public float Speed;                //弾速
    public float Homing;               //ホーミング性能
    public float Late;                 //発射間隔
    public float DownPower;              //ダウン値
    public float Range;                 //射程
    public float UseTime;               //行動不能時間
    public float CoolDownTime;           //武器の使用不可時間
}

