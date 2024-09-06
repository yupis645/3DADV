using System;
using UnityEngine.Assertions;

//----------------------------------------------------------------------------
//                      アクセス制限をかけたシングルトンパターン(MonoBehaviourを継承しない)
//
// 「唯一のインスタンス」を保証しつつ、公開範囲を派生クラスまでに限定した基底クラス
// 
//----------------------------------------------------------------------------


public abstract class ProtectedSingletonBaseClass<T> : IDisposable where T : class, new()
{
    private static T instance;       //保持するインスタンス

    protected static T Instance      //公開するインスタンス(派生クラスまでのみアクセス可能)
    {
        get
        {
            Assert.IsNotNull(instance, "There is no object attached " + typeof(T).Name);
            return instance;
        }
    }

    protected static bool IsExist() => instance != null;

    //======================================================
    //                  初期化
    //
    // インスタンスのチェックとGameObjectの保護
    // 1.インスタンスのチェック。インスタンスが見つかれば破棄する
    // 2.クラスのインスタンスの生成
    // 3.DontDestroyOnLoadの設定
    // 4.追加で初期化メソッドを実行する
    //======================================================
    protected ProtectedSingletonBaseClass()
    {
        if (instance != null && instance != this)
        {
            return;
        }
        instance = this as T;

        CustomConstructor();  // 派生先の初期化処理
    }
    //======================================================
    //                  初期化時の追加処理
    //
    // 派生先でも初期化処理を書くためのAPI
    //======================================================
    protected virtual void CustomConstructor()
    {

    }

    // Destroy時処理

    //======================================================
    //          オブジェクト破棄時の処理
    //======================================================
    public virtual void Dispose()
    {
        OnDestroyProcess();
        instance = null;
    }


    //======================================================
    //                  初期化時の追加処理
    //
    // 派生先でも初期化処理を書くためのAPI
    //======================================================
    protected virtual void OnDestroyProcess()
    {
    }
}