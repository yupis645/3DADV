using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class AssembledUnit 
{
    public CharacterStatus chara;           //キャラクターの元ステータス
    public  ArmsArmorStatus arms;           //アームパーツステータス
    public LegsArmorStatus legs;            //レッグパーツステータス
    public WeaponStatus[] weapons = new WeaponStatus[4];    //腕 & 背中の武器パーツステータス


    public CharacterStatus status;          //全てのパーツを合わせたユニットとしてのステータス

    //=======================================================
    //          初期化
    //
    //
    //=======================================================
    public void initialize()
    {
        Debug.Log("initialize実行");
        status = chara;                             //ユニットステータスにキャラクターステータスを代入
        status.HP = chara.HP + arms.HP + legs.HP;   //アーム、レッグパーツステータスからHPをユニットステータスに加算
        status.MoveSpeed += legs.AddSpeed;          //ユニットステータスにレッグパーツの速度上昇を付与する
    }

}
