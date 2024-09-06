using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;


public static  class FlagsBitUtility 
{
    //----------------------------------------------------------------------------------------------------------------------
    //                                              ���ۃ��\�b�h

    //=========================================================================================
    //          Enum��flags�ł��邩���m�F���钊�ۃ��\�b�h
    //=========================================================================================
    public static void CheckHaveFlags<T>(T flags) where T : struct, Enum
    {
        if (!typeof(T).IsDefined(typeof(FlagsAttribute), false))
        {
            throw new ArgumentException($"{typeof(T).Name} �� Flags �������Ă��܂���.");
        }
    }

    //=========================================================================================
    //       Enum��int�ɕϊ����Abit���Z��Ă�enum�ɖ߂����ۃ��\�b�h 
    // �I�[�o�[���[�h���g�p
    // �� : �w�肳���bit���P�̂̏ꍇ
    // �� : �w�肳���bit�������̏ꍇ
    //=========================================================================================
    /// <summary>
    /// �w�肳���bit����̏ꍇ
    /// </summary>
    private static T PerformBitOperation<T>(T flags, T flagsbit, Func<int, int, int> operation) where T : struct, Enum
    {
        int flagsValue = Convert.ToInt32(flags);        //enum��int�^�ɕϊ�����

        flagsValue = operation(flagsValue, Convert.ToInt32(flagsbit)); //Func�ɂ���ăr�b�g���Z�����s���Aint�^��flagsValue�ɑ������

        return (T)(object)flagsValue;                   //��x�I�u�W�F�N�g�^�ɖ߂��Ă���T(Enum)�^�ɖ߂��Ȃ���Ԃ�
    }

    /// <summary>
    //�w�肳���bit�������̏ꍇ
    /// </summary>
    private static T PerformBitOperation<T>(T flags, T[] flagsbit, Func<int, int, int> operation) where T : struct, Enum
    {
        int flagsValue = Convert.ToInt32(flags);        //enum��int�^�ɕϊ�����

        foreach (var state in flagsbit)
        {
            flagsValue = operation(flagsValue, Convert.ToInt32(state)); //Func�ɂ���ăr�b�g���Z�����s���Aint�^��flagsValue���X�V���Ă���
        }

        return (T)(object)flagsValue;                   //��x�I�u�W�F�N�g�^�ɖ߂��Ă���T(Enum)�^�ɖ߂��Ȃ���Ԃ�
    }

    //----------------------------------------------------------------------------------------------------------------------
    //                                            �ÓI���\�b�h

    //=========================================================================================
    //          Flags�̃r�b�g��'0'�ɂ���
    //����1 : ���삷��Flags  ����2 : ���삷��bit�w��
    //=========================================================================================
    public static void RemoveFlagsBit<T>(ref T flags, params T[] flagsbit) where T : struct, Enum
    {
        // Flags�������t���Ă��邩�m�F
        CheckHaveFlags(flags);

        // �r�b�g���Z�� AND NOT (a & ~b) �ōs��
        flags = PerformBitOperation(flags, flagsbit, (a, b) => a & ~b);
    }

    //=========================================================================================
    //          Flags�̃r�b�g��'1'�ɂ���
    //����1 : ���삷��Flags  ����2 : ���삷��bit�w��
    //=========================================================================================
    public static void AddFlagsBit<T>(ref T flags, params T[] flagsbit) where T : struct, Enum
    {
        // Flags�������t���Ă��邩�m�F
        CheckHaveFlags(flags);

        // �r�b�g���Z�� AND NOT (a & ~b) �ōs��
        flags = PerformBitOperation(flags, flagsbit, (a, b) => a | b);
    }


}
