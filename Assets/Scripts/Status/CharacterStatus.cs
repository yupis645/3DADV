using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterStatus", menuName = "StatusData/CharacterStatus")]
public class CharacterStatus : ScriptableObject
{
    [Header("ID / ���O")]
    public int ID;
    public string Name;

    [Header("�X�e�[�^�X�p�����[�^�[")]
    public int HP;
    public int ATK;
    public int DEF;
    public int ARMOR;
    [Header("�ړ����x(�����x) / �ō����x")]
    public float MoveSpeed = 0;           //1�t���[��������̈ړ����x(����)
    public float MoveSpeedLimit = 0;      //�ō����x

    [Header("�_�b�V�����x / �_�b�V������")]
    public float DashSpeed = 0;        //�������㑬�x
    public float DashRange = 0;      //�󒆃_�b�V�����x

    [Header("�󒆃_�b�V�����x / �󒆃_�b�V������")]
    public float AirDashSpeed = 0;      //�󒆃_�b�V�����x
    public float AirRange = 0;      //�󒆃_�b�V�����x

    [Header("�W�����v��")]
    public float JumpPower = 0;           //�W�����v��

    [Header("�󒆃W�����v��")]
    public int MaxJumpCount = 0;          //�ő�W�����v��
    public enum G_Dashmotion
    {
        step, roll, shortstep, boost,
    }
    public enum A_Dashmotion
    {
        step, airjump, flight, dropjump,
    }
    [Header("�n��_�b�V�^�C�v")]
    public G_Dashmotion G_DashType;

    [Header("�󒆃_�b�V�^�C�v")]
    public A_Dashmotion A_DashType;

    //=======================================================
    //          ������
    //
    //
    //=======================================================
    virtual public void Initialize()
    {
        Debug.Log("initialize���s");
    }
}
