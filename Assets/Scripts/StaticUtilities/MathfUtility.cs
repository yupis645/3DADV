using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class MathfUtility
{

    //==========================================================================
    //              Func�Ŏ󂯎�����������Ɍv�Z���郁�\�b�h
    // ���� 1.���ӂ̒l , 2.�E�ӂ̒l , 3.�l�����x�l�𒴂����Ƃ��ɕԂ��l
    //      4.�v�Z�� ,
    //      5.�ŏ��l(�����Ȃ��ꍇ�͍ŏ��l�̎w��Ȃ�) , 6.�ő�l(�����Ȃ��ꍇ�͍ő�l�̎w��Ȃ�)
    //==========================================================================

    public static float MathfClampCustom(float a,float b, Func<float, float,float> cal,float overvalue,float min = -float.MaxValue,float max = float.MaxValue)
    {
        float result = cal(a, b);             //Func�Ŏw�肳�ꂽ�v�Z�����s

        if (Mathf.Abs(result) >= min && Mathf.Abs(result) <= max)
        {
            return result;                       //�l���͈͓��ɂ���Ȃ�v�Z���ʂ����̂܂ܕԂ�
        }

        return overvalue;

    }



}
