using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponStatus", menuName = "StatusData/WeaponStatus")]
public class WeaponStatus : ScriptableObject
{

    [Header("ID / ���O")]
    public int WeaponID;
    public string WeaponName;

    enum weaponCategory
    {
        firearms , boms , missile , cannon , melee , assist     //�e��,�{��,�~�T�C��,�L���m���C,�ߐڕ���,�A�V�X�g
    }
    [SerializeField] weaponCategory weaponType;

    [Header("�X�e�[�^�X�p�����[�^�[")]
    public float  AttackPower;            //�U����
    public float Speed;                //�e��
    public float Homing;               //�z�[�~���O���\
    public float Late;                 //���ˊԊu
    public float DownPower;              //�_�E���l
    public float Range;                 //�˒�
    public float UseTime;               //�s���s�\����
    public float CoolDownTime;           //����̎g�p�s����
}

