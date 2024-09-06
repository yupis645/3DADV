using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterStatus", menuName = "StatusData/CharacterStatus")]
public class CharacterStatus : ScriptableObject
{
    [Header("ID / 名前")]
    public int ID;
    public string Name;

    [Header("ステータスパラメーター")]
    public int HP;
    public int ATK;
    public int DEF;
    public int ARMOR;
    [Header("移動速度(加速度) / 最高速度")]
    public float MoveSpeed = 0;           //1フレーム当たりの移動速度(加速)
    public float MoveSpeedLimit = 0;      //最高速度

    [Header("ダッシュ速度 / ダッシュ距離")]
    public float DashSpeed = 0;        //だっしゅ速度
    public float DashRange = 0;      //空中ダッシュ速度

    [Header("空中ダッシュ速度 / 空中ダッシュ距離")]
    public float AirDashSpeed = 0;      //空中ダッシュ速度
    public float AirRange = 0;      //空中ダッシュ速度

    [Header("ジャンプ力")]
    public float JumpPower = 0;           //ジャンプ力

    [Header("空中ジャンプ回数")]
    public int MaxJumpCount = 0;          //最大ジャンプ回数
    public enum G_Dashmotion
    {
        step, roll, shortstep, boost,
    }
    public enum A_Dashmotion
    {
        step, airjump, flight, dropjump,
    }
    [Header("地上ダッシタイプ")]
    public G_Dashmotion G_DashType;

    [Header("空中ダッシタイプ")]
    public A_Dashmotion A_DashType;

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
