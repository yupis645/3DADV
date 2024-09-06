using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class BaseCharacterParameter
{

    [Header("ステータスパラメーター")]
    [SerializeField] private int HP;
    [SerializeField] private int ATK;
    [SerializeField] private int DEF;
    [SerializeField] private int ARMOR;
    [SerializeField] private int GOUNDSPEED;
    [SerializeField] private int AIRSPEED;
    [Header("移動速度(加速度) / 最高速度")]
    [SerializeField] private float moveSpeed = 0;           //1フレーム当たりの移動速度(加速)
    [SerializeField] private float moveSpeedLimit = 0;      //最高速度

    [Header("ダッシュ速度 / ダッシュ距離")]
    [SerializeField] private float dashSpeed = 0;        //だっしゅ速度
    [SerializeField] private float dashRange = 0;      //空中ダッシュ速度

    [Header("空中ダッシュ速度 / 空中ダッシュ距離")]
    [SerializeField] private float airDashSpeed = 0;      //空中ダッシュ速度
    [SerializeField] private float airRange = 0;      //空中ダッシュ速度

    [Header("ジャンプ力")]
    [SerializeField] private float jumpPower = 0;           //ジャンプ力

    [Header("空中ジャンプ回数")]
    [SerializeField] private int maxjumpCount = 0;          //最大ジャンプ回数

    public int Hp { get => HP; set => HP = value; }
    public int Atk { get => ATK; set => ATK = value; }
    public int Def { get => DEF; set => DEF = value; }
    public int Armor { get => ARMOR; set => ARMOR = value; }
    public int GroundSpeed { get => GOUNDSPEED; set => GOUNDSPEED = value; }
    public int AirSpeed { get => AIRSPEED; set => AIRSPEED = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float MoveSpeedLimit { get => moveSpeedLimit; set => moveSpeedLimit = value; }
    public float DashSpeed { get => dashSpeed; set => dashSpeed = value; }
    public float DashRange { get => dashRange; set => dashRange = value; }
    public float AirDashSpeed { get => airDashSpeed; set => airDashSpeed = value; }
    public float AirRange { get => airRange; set => airRange = value; }
    public float JumpPower { get => jumpPower; set => jumpPower = value; }
    public int MaxjumpCount { get => maxjumpCount; set => maxjumpCount = value; }


}
