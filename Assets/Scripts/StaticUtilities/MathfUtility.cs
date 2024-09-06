using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class MathfUtility
{

    //==========================================================================
    //              Funcで受け取った式を元に計算するメソッド
    // 引数 1.左辺の値 , 2.右辺の値 , 3.値が限度値を超えたときに返す値
    //      4.計算式 ,
    //      5.最小値(書かない場合は最小値の指定なし) , 6.最大値(書かない場合は最大値の指定なし)
    //==========================================================================

    public static float MathfClampCustom(float a,float b, Func<float, float,float> cal,float overvalue,float min = -float.MaxValue,float max = float.MaxValue)
    {
        float result = cal(a, b);             //Funcで指定された計算を実行

        if (Mathf.Abs(result) >= min && Mathf.Abs(result) <= max)
        {
            return result;                       //値が範囲内にあるなら計算結果をそのまま返す
        }

        return overvalue;

    }



}
