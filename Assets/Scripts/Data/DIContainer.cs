using System;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------------------------------
//                          DIコンテナ(インスタンスの管理をするクラス)
//
// インスタンスを格納するクラス。メソッドを介してインスタンスの登録や譲渡させることができる
//
//----------------------------------------------------------------------------------------------------------------


public class DIContainer
{
    //registrations：型とその型のインスタンスを生成するためのファクトリメソッドを格納する辞書。キーは型 (Type)、値はインスタンスを生成するためのファクトリメソッド (Func<object>)。
    private readonly Dictionary<Type, Func<object>> registrations = new Dictionary<Type, Func<object>>();


    //===============================================================
    //              ファクトリメソッドを登録
    //
    // 型 T のインスタンスを生成するファクトリメソッドを登録する。
    //    パラメータ：factory - 型 T のインスタンスを生成するファクトリメソッド。
    // 1.registrations 辞書に、キーとして型 T、値としてファクトリメソッドを追加します。
    //ファクトリメソッドは Func<object> として保存されるため、キャストが行う
    //===============================================================

    public void Register<T>(Func<T> factory) where T : class
    {
        registrations[typeof(T)] = () => factory();
    }

    //===============================================================
    //              指定された型を返す
    //
    //登録されたファクトリメソッドを使用して、型 T のインスタンスを解決（生成）します。
    // 1.registrations 辞書から、キーとして型 T を検索。
    // 2.対応するファクトリメソッドが見つかれば、それを実行してインスタンスを生成して返す。
    // 3.見つからない場合（登録されていない場合）、例外をスローする。
    //===============================================================

    public T Resolve<T>() where T : class
    {
        //辞書の中を探し、同じ型が見つかればそれをインスタスタンス化して渡す
        if (registrations.TryGetValue(typeof(T), out var factory))
        {
             return (T)factory();
        }

        //見つからない場合は例外をスローする
        throw new InvalidOperationException($"No registration for {typeof(T)}");
    }

    //===============================================================
    //             登録したインスタンスの更新
    //
    // 登録されているインスタンスを引数のインスタンスで上書きする
    // 1.登録されたインスタンスがあるかどうかをチェック
    // 2.登録されたインスタンスを上書き
    //===============================================================
    public void SetInstance<T>(T instance) where T : class
    {
        // 1.登録されたインスタンスがあるかどうかをチェック
        if (!registrations.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException($"No registration for {typeof(T)}");
        }

        // 2.登録されたインスタンスを上書き
        registrations[typeof(T)] = () => instance;
    }
    //===============================================================
    //             登録している中身をログに表示させる
    //
    // デバックなどで使用する。現在登録しているインスタンスを確認する
    //===============================================================

    public void cheark()
    {
        // Dictionaryの内容をデバッグログに出力
        foreach (var kvp in registrations)
        {
            Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
        }
    }
}