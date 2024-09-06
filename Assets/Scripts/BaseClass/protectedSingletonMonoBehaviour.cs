using System;
using UnityEngine;
using UnityEngine.Assertions;

//----------------------------------------------------------------------------
//                      アクセス制限をかけたシングルトンパターン(MonoBehaviour継承)
//
// 「唯一のインスタンス」を保証しつつ、公開範囲を派生クラスまでに限定した基底クラス
// マネージャークラスでMonoBehaviourを継承する(オブジェクトが生成される)クラスとして使うことを想定
//
// ※ DontDestroyOnLoadやAwakeメソッドを使っているため、派生クラスで宣言しないように注意!!
//----------------------------------------------------------------------------


public abstract class protectedSingletonMonoBehaviour<TYpe> : MonoBehaviour, IDisposable where TYpe : MonoBehaviour
{
    private static  TYpe instance;     //インスタンス

    protected static TYpe Instance      //公開するインスタンス(派生クラスまでのみアクセス可能)
    {
        get
        {
            Assert.IsNotNull(instance, "There is no object attached " + typeof(TYpe).Name);
            return instance;
        }
    }

    // 存在チェック(True:存在, False:インスタンスが無い)
    public static bool IsExist() { return instance != null; }

    //======================================================
    //                  初期化
    //
    // インスタンスのチェックとGameObjectの保護
    // 1.インスタンスのチェック。インスタンスが見つかれば破棄する
    // 2.クラスのインスタンスの生成
    // 3.DontDestroyOnLoadの設定
    // 4.追加で初期化メソッドを実行する
    //======================================================

    protected virtual void Awake()
    {
        // 1.インスタンスのチェック。インスタンスが見つかれば破棄する
        if (instance != null && instance != this)
        {
            Debug.Log($"インスタンス生成済み。新たに作られた生成物{this.gameObject.name}を破棄します");
            Destroy(this.gameObject);  // 新しいインスタンスを破棄する
            return;
        }
        
        // 2.クラスのインスタンスの生成
        instance = this as TYpe;

        // 3.DontDestroyOnLoadの設定
        DontDestroyOnLoad(this.gameObject);  // シーンを跨いでも削除されないようにする

        // 4.追加で初期化メソッドを実行する
        OnAwakeProcess();  
    }

    //======================================================
    //                  初期化時の追加処理
    //
    // 派生先でも初期化処理を書くためのAPI
    //======================================================
    protected virtual void OnAwakeProcess()
    {
        //派生クラスで記載 & ここでは書かない!
    }

    //======================================================
    //          オブジェクト破棄時の処理
    //======================================================
    private void OnDestroy()
    {
        // 自身以外のインスタンスが作成→即時破棄されるときに間違って実行されないようにブロック
        if (instance != (this as TYpe)) return;
        OnDestroyProcess();
        Dispose();
    }

    //======================================================
    //          オブジェクトの破棄時の追加処理
    //
    // / 派生先でも初期化処理を書くためのAPI
    //======================================================
    // 派生先でも破棄処理を書くためのAPI
    protected virtual void OnDestroyProcess()
    {
        //派生クラスで記載 & ここでは書かない!
    }

    //======================================================
    //                 破棄時の処理
    //======================================================
    public virtual void Dispose()
    {
        // もしinstanceがあるならそれを破棄する
        if (IsExist()) instance = null;
    }
}