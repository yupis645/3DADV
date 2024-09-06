using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class BaseCharacterParameter
{

    [Header("�X�e�[�^�X�p�����[�^�[")]
    [SerializeField] private int HP;
    [SerializeField] private int ATK;
    [SerializeField] private int DEF;
    [SerializeField] private int ARMOR;
    [SerializeField] private int GOUNDSPEED;
    [SerializeField] private int AIRSPEED;
    [Header("�ړ����x(�����x) / �ō����x")]
    [SerializeField] private float moveSpeed = 0;           //1�t���[��������̈ړ����x(����)
    [SerializeField] private float moveSpeedLimit = 0;      //�ō����x

    [Header("�_�b�V�����x / �_�b�V������")]
    [SerializeField] private float dashSpeed = 0;        //�������㑬�x
    [SerializeField] private float dashRange = 0;      //�󒆃_�b�V�����x

    [Header("�󒆃_�b�V�����x / �󒆃_�b�V������")]
    [SerializeField] private float airDashSpeed = 0;      //�󒆃_�b�V�����x
    [SerializeField] private float airRange = 0;      //�󒆃_�b�V�����x

    [Header("�W�����v��")]
    [SerializeField] private float jumpPower = 0;           //�W�����v��

    [Header("�󒆃W�����v��")]
    [SerializeField] private int maxjumpCount = 0;          //�ő�W�����v��

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
