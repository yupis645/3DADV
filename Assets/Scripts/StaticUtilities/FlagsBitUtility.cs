using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;


public static  class FlagsBitUtility 
{
    //----------------------------------------------------------------------------------------------------------------------
    //                                              抽象メソッド

    //=========================================================================================
    //          Enumがflagsであるかを確認する抽象メソッド
    //=========================================================================================
    public static void CheckHaveFlags<T>(T flags) where T : struct, Enum
    {
        if (!typeof(T).IsDefined(typeof(FlagsAttribute), false))
        {
            throw new ArgumentException($"{typeof(T).Name} は Flags を持っていません.");
        }
    }

    //=========================================================================================
    //       Enumをintに変換し、bit演算後再びenumに戻す抽象メソッド 
    // オーバーロードを使用
    // 上 : 指定されるbitが単体の場合
    // 下 : 指定されるbitが複数の場合
    //=========================================================================================
    /// <summary>
    /// 指定されるbitが一つの場合
    /// </summary>
    private static T PerformBitOperation<T>(T flags, T flagsbit, Func<int, int, int> operation) where T : struct, Enum
    {
        int flagsValue = Convert.ToInt32(flags);        //enumをint型に変換する

        flagsValue = operation(flagsValue, Convert.ToInt32(flagsbit)); //Funcによってビット演算を実行し、int型のflagsValueに代入する

        return (T)(object)flagsValue;                   //一度オブジェクト型に戻してからT(Enum)型に戻しながら返す
    }

    /// <summary>
    //指定されるbitが複数の場合
    /// </summary>
    private static T PerformBitOperation<T>(T flags, T[] flagsbit, Func<int, int, int> operation) where T : struct, Enum
    {
        int flagsValue = Convert.ToInt32(flags);        //enumをint型に変換する

        foreach (var state in flagsbit)
        {
            flagsValue = operation(flagsValue, Convert.ToInt32(state)); //Funcによってビット演算を実行し、int型のflagsValueを更新していく
        }

        return (T)(object)flagsValue;                   //一度オブジェクト型に戻してからT(Enum)型に戻しながら返す
    }

    //----------------------------------------------------------------------------------------------------------------------
    //                                            静的メソッド

    //=========================================================================================
    //          Flagsのビットを'0'にする
    //引数1 : 操作するFlags  引数2 : 操作するbit指定
    //=========================================================================================
    public static void RemoveFlagsBit<T>(ref T flags, params T[] flagsbit) where T : struct, Enum
    {
        // Flags属性が付いているか確認
        CheckHaveFlags(flags);

        // ビット演算を AND NOT (a & ~b) で行う
        flags = PerformBitOperation(flags, flagsbit, (a, b) => a & ~b);
    }

    //=========================================================================================
    //          Flagsのビットを'1'にする
    //引数1 : 操作するFlags  引数2 : 操作するbit指定
    //=========================================================================================
    public static void AddFlagsBit<T>(ref T flags, params T[] flagsbit) where T : struct, Enum
    {
        // Flags属性が付いているか確認
        CheckHaveFlags(flags);

        // ビット演算を AND NOT (a & ~b) で行う
        flags = PerformBitOperation(flags, flagsbit, (a, b) => a | b);
    }


}
