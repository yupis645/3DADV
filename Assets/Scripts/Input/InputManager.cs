
//--------------------------------------------------------------------------------------------------------
//                        入力クラス(マネージャークラス)
//
// 概要 : IInputProviderインターフェースを継承した実装クラス。
// 役割 : 入力機器からの入力を bool もしく float で取得する。
//
// 現在はキーボード、ゲームパッド(xboxコントローラー)の二つを切り替えて使うことができる
//--------------------------------------------------------------------------------------------------------

public class InputManager : protectedSingletonMonoBehaviour<InputManager>, IInputManager
{
    //*************************************************************************************************************************
    //                                          メソッド

    //===========================================================================================
    //                      初期化メソッド
    //===========================================================================================
    public int Initialize()
    {
        //InstanceManager.Instance.SetInputProviderInstance<KeyboardInputProvider>();

        return 0;
    }

    //===========================================================================================
    //                      初期設定メソッド
    //===========================================================================================
    public int SetUp()
    {

        return 0;
    }



    //===========================================================================================
    //                      ★破棄メソッド
    //
    // 基底クラスのOndestroyメソッドの中にある実行メソッド。
    //===========================================================================================
    // 派生先でも破棄処理を書くためのAPI
    protected override void OnDestroyProcess()
    {
    }


    //=====================================================================
    //                左右の入力値を取得(プレイヤーオブジェクト操作)
    //=====================================================================
    public float PlayerMoveHorizontal()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerMoveHorizontal();

    }

    //=====================================================================
    //                上下の入力値を取得(プレイヤーオブジェクト操作)
    //=====================================================================
    public float PlayerMoveVertical()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerMoveVertical();

    }

    //=====================================================================
    //                ジャンプの入力を取得(プレイヤーオブジェクト操作)
    //=====================================================================
    public bool PlayerJumpButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerJumpButton();

    }
    //=====================================================================
    //                ダッシュボタンの入力状態を監視(プレイヤー操作)
    //=====================================================================
    public bool PlayerDashButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerDashButton();
    }
    //=====================================================================
    //                左腕武器ボタンの入力状態を監視(プレイヤー操作)
    //=====================================================================
    public bool PlayerLeftArmsButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerLeftArmsButton();
    }
    //プレイヤーの左腕の武器使用
    //=====================================================================
    //                右腕武器ボタンの入力状態を監視(プレイヤー操作)
    //=====================================================================
    public bool PlayerRightArmsButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerRightArmsButton();
    }

    //=====================================================================
    //                背面左武器ボタンの入力状態を監視(プレイヤー操作)
    //=====================================================================
    public bool PlayerLeftBackArmsButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerLeftBackArmsButton();
    }
    //=====================================================================
    //                背面右武器ボタンの入力状態を監視(プレイヤー操作)
    //=====================================================================
    public bool PlayerRightBackArmsButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerRightBackArmsButton();
    }



    //=====================================================================
    //                左右の入力値を取得(UI操作)
    //=====================================================================
    public float UiCursorMoveHorizontal()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().UiCursorMoveHorizontal();

    }

    //=====================================================================
    //                左右の入力状態を取得(UI操作)
    //=====================================================================
    public bool HorizontalInputDown()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().HorizontalInputDown();
      
    }

    //=====================================================================
    //               上下の入力値を取得(UI操作)
    //=====================================================================
    public float UiCursorMoveVertical()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().UiCursorMoveVertical();

    }

    //=====================================================================
    //                上下の入力状態を取得(UI操作)
    //=====================================================================
    public bool VerticalInputDown()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().VerticalInputDown();
    }

    //=====================================================================
    //                決定ボタンの入力状態を取得(UI操作)
    //=====================================================================
    public bool UiSelectButton()
    {
        if (InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().UiSelectButton())
        {
            return true;
        }

        return false;
    }

    //=====================================================================
    //                キャンセルボタンの入力状態を取得(UI操作)
    //=====================================================================
    public bool UiCancelButton()
    {
        if (InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().UiCancelButton())
        {
            return true;
        }

        return false;
    }

    //=====================================================================
    //                ポーズボタンの入力状態を取得(UI操作)
    //=====================================================================
    public bool PoseButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PoseButton();
    }
    public bool GameCloseButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().GameCloseButton();
    }


    //=====================================================================
    //                入力状態の監視
    //
    // 現在の入力モードとは異なる入力機器からの入力を取得する。
    // キーボードモードならゲームパッドの入力を監視
    // ゲームパットならキーボードの入力を監視
    //=====================================================================
    public bool OtherInputCheak()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().OtherInputCheak();
    }                




}
