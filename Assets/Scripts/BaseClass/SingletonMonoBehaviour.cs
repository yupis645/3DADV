
using System;
using UnityEngine;
using UnityEngine.Assertions;

//-------------------------------------------------------------------------------------------------------
//                          MonoBehaviour継承したSingleton
//-------------------------------------------------------------------------------------------------------
public abstract class SingletonMonoBehaviour<TYpe> : MonoBehaviour, IDisposable where TYpe : MonoBehaviour
{
    private static TYpe instance;

    public static TYpe Instance
    {
        get
        {
            Assert.IsNotNull(instance, "There is no object attached " + typeof(TYpe).Name);
            return instance;
        }
    }

    //存在チェック(True:存在, False:インスタンスが無い)
    public static bool IsExist() { return instance != null; }

    private void Awake()
    {
        if (instance != null && instance.gameObject != null)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);        //シーンを跨いでも削除されないようにする

        instance = this as TYpe;        //インスタンスを作成する

        OnAwakeProcess();   //派生先の初期化処理
    }

    //====================================================================================
    //              派生先でも初期化処理を書くためのAPI
    //====================================================================================
    protected virtual void OnAwakeProcess() 
    {

    }

    //====================================================================================
    ///                     Destroy時処理
    ///
    //         派生先で実行漏れが無いように意図的にPrivate
    //====================================================================================
    private void OnDestroy()
    {
        // 自身以外のインスタンスが作成→即時破棄されるときに間違って実行されないようにブロック
        if (instance != (this as TYpe)) return;
        OnDestroyProcess();
        Dispose();
    }

    //====================================================================================
    ///                     派生先でも破棄処理を書くためのAPI
    //====================================================================================
    protected virtual void OnDestroyProcess()
    {
        //純粋仮想関数なのでここでは処理を書かないこと!!  
    }

    //====================================================================================
    ///                     もしinstanceがあるならそれを破棄する
    //====================================================================================
    public virtual void Dispose()
    {
        if (IsExist()) instance = null;
    }

}
