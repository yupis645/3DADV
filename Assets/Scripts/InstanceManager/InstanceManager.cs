using UnityEngine;

//------------------------------------------------------------------------------------------------------------------------------------------
//                    シーン全体のインスタンスを管理するマネージャークラス(シングルトン)
//
// マネージャークラスなどのシーン全体で共有するインスタンスをDIコンテナへ登録、呼び出し、インスタンスのセットなどをする
// また、prefubやテキストデータなども格納しており、データを取得するのにはこのクラスを経由する必要がある。
// ※基底クラスでシングルトンの実装をしており、Awakeを使用しているのでここでは再定義しないこと。
//------------------------------------------------------------------------------------------------------------------------------------------

public class InstanceManager : SingletonMonoBehaviour<InstanceManager>
{
    //****************************************************************************************************************************
    //                                          変数宣言
    
    //マネージャークラスのprefub
    [SerializeField] GameObject GameManagerPrefub;  
    [SerializeField] GameObject InputManagerPrefub;


    //DIコンテナのインスタンス
    private DIContainer container = new DIContainer();

    //ゲーム全体で共有するデータ群のインスタンス取得(inspectorより取得)
    [SerializeField]public MasterData _masterData;    //全シーンで共有するデータクラス

    //[SerializeField] SoundData _soundData;

    // 外部に公開するデータ群のgetter定義
    public MasterData masterData { get => _masterData; }

    //*******************************************************************************************************
    //                          メソッド

    //=====================================================================
    //                  初期化
    //
    // 基底クラスのAwakeクラスにある追加メソッドに書き込んで初期化する。
    // 1.マネージャークラスのインスタンス化
    // 2.作成したマネージャークラスのインスタンスをDIコンテナに登録
    // 3.入力機器(キーボードやゲームパッド)の入力インターフェースを登録
    // 4.データクラスをインスタンス化する
    //=====================================================================
    protected override void OnAwakeProcess()
    {
        // 1.マネージャークラスのインスタンス化
        GameObject im = Instantiate(InputManagerPrefub);    //インプットマネ-ジャー
        GameObject gm = Instantiate(GameManagerPrefub);     //ゲームマネージャー

        // 2.作成したマネージャークラスのインスタンスをDIコンテナに登録
        container.Register<InstanceManager>(() => this);                                //自分(インスタンスマネージャー)
        container.Register<IInputManager>(() => im.GetComponent<InputManager>());       //インプットマネージャ-
        container.Register<IGameManager>(() => gm.GetComponent<GameManager>());         //ゲームマネージャー


        // 3.入力機器(キーボードやゲームパッド)の入力インターフェースを登録
        container.Register<KeyboardInputProvider>(() => new KeyboardInputProvider());       //キーボード用
        container.Register<GamePadInputProvider>(() => new GamePadInputProvider());         //ゲームパッド用

        // 4.データクラスをインスタンス化する
        _masterData = new MasterData();

    }

    //=====================================================================
    //                  初期設定
    //
    // 差し替え可能なインスタンスなどの初期状態にする
    // 1.入力機器をキーボードを初期設定にする
    //=====================================================================
    private void Start()
    {
        // 1.入力機器をキーボードを初期設定にする
        container.Register<IInputProvider>(() => GetInputProviderInstance<KeyboardInputProvider>());         //入力機器の初期設定
   
        // 2.ゲームマネージャーの処理を始める。(ゲームの開始)
        GetGameManagerInstance<IGameManager>().Initialize();
        GetGameManagerInstance<IGameManager>().SetUp();

    }

    //=====================================================================
    //                  入力機器の入れ替え
    //
    // 現在の入力モードから別の入力機器に変更する。
    //====================================================================
    public void ChangeInputMode()
    {

        // 1.現在の入力モードがキーボードモードなら
        if (GetInputProviderInstance<IInputProvider>() is KeyboardInputProvider)
        {
            container.SetInstance<IInputProvider>(GetInputProviderInstance<GamePadInputProvider>());
        }

        // 2.現在の入力モードがゲームパッドモードなら
        else if (GetInputProviderInstance<IInputProvider>() is GamePadInputProvider)
        {
            container.SetInstance<IInputProvider>(GetInputProviderInstance<KeyboardInputProvider>());
        }
    }

    
    //***********************************************************************************************************************
    //                              DIコンテナからの取得メソッド


    public T GetGameManagerInstance<T>() where T : class,IGameManager
    {
        return container.Resolve<T>();
    }
    public T GetInstanceManagerInstance<T>() where T : InstanceManager
    {
        return container.Resolve<T>();
    }

    public T GetInputManagerInstance<T>() where T : class,IInputManager
    {
        return container.Resolve<T>();
    }

     public T GetInputProviderInstance<T>() where T : class,IInputProvider
    {
        return container.Resolve<T>();
    }




}


