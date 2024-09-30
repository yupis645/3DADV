using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class AssembledUnit 
{
    public CharacterStatus chara;           //�L�����N�^�[�̌��X�e�[�^�X
    public  ArmsArmorStatus arms;           //�A�[���p�[�c�X�e�[�^�X
    public LegsArmorStatus legs;            //���b�O�p�[�c�X�e�[�^�X
    public WeaponStatus[] weapons = new WeaponStatus[4];    //�r & �w���̕���p�[�c�X�e�[�^�X


    public CharacterStatus status;          //�S�Ẵp�[�c�����킹�����j�b�g�Ƃ��ẴX�e�[�^�X

    //=======================================================
    //          ������
    //
    //
    //=======================================================
    public void initialize()
    {
        Debug.Log("initialize���s");
        status = chara;                             //���j�b�g�X�e�[�^�X�ɃL�����N�^�[�X�e�[�^�X����
        status.HP = chara.HP + arms.HP + legs.HP;   //�A�[���A���b�O�p�[�c�X�e�[�^�X����HP�����j�b�g�X�e�[�^�X�ɉ��Z
        status.MoveSpeed += legs.AddSpeed;          //���j�b�g�X�e�[�^�X�Ƀ��b�O�p�[�c�̑��x�㏸��t�^����
    }

}
