using UnityEngine;

//--------------------------------------------------------------------------------------------------------
//                        ゲームパッドの入力実装クラス
//
// IInputProviderインターフェースを継承した入力クラス。
// ゲームパットからの入力を受け取る
//--------------------------------------------------------------------------------------------------------

public class GamePadInputProvider : IInputProvider
{
    static bool hori = false;
    static bool ver = false;

    //=====================================================================
    //                左右の入力状態を監視(UI操作)
    //
    // 十字キー、左スティック両方の入力をを監視する
    //=====================================================================
    public bool HorizontalInputDown()
    {
        if (Input.GetAxisRaw("L_Stick_H") == 0 && Input.GetAxisRaw("D_Pad_H") == 0) hori = false;
        else
        {
            if (hori == false)
            {
                hori = true;
                return true;
            }
        }

        return false;
    }

    //=====================================================================
    //                上下の入力状態を監視(UI操作)
    //
    // 十字キー、左スティック両方の入力をを監視する
    //=====================================================================
    public bool VerticalInputDown()
    {
        {

            if (Input.GetAxisRaw("L_Stick_V") == 0 && Input.GetAxisRaw("D_Pad_V") == 0) ver = false;
            else
            {
                if (ver == false)
                {
                    ver = true;
                    return true;
                }
            }

            return false;
        }
    }

    //=====================================================================
    //                左右の入力値を取得(プレイヤー操作)
    //
    // 十字キー、左スティック両方の入力を取得する
    //=====================================================================
    public float PlayerMoveHorizontal()
    {
        if (Input.GetAxis("L_Stick_H") != 0)
        {
            return Input.GetAxis("L_Stick_H");
        }
        
        return Input.GetAxis("D_Pad_H");
    }

    //=====================================================================
    //                上下の入力値を取得(プレイヤー操作)
    //
    // 十字キー、左スティック両方の入力を取得する
    //=====================================================================
    public float PlayerMoveVertical()
    {
        if (Input.GetAxis("L_Stick_V") != 0)
        {
            return Input.GetAxis("L_Stick_V");

        }
        return Input.GetAxis("D_Pad_V");
    }

    //=====================================================================
    //                ジャンプボタンの入力状態を監視(プレイヤー操作)
    //=====================================================================
    public bool PlayerJumpButton()
    {
        return Input.GetButtonDown("A_button");
    }            //プレイヤーのジャンプ


    //=====================================================================
    //                ダッシュボタンの入力状態を監視(プレイヤー操作)
    //=====================================================================
    public bool PlayerDashButton()
    {

        return true;
    }
    //=====================================================================
    //                左腕武器ボタンの入力状態を監視(プレイヤー操作)
    //=====================================================================
    public bool PlayerLeftArmsButton()
    {
        return false;
    }
    //プレイヤーの左腕の武器使用
    //=====================================================================
    //                右腕武器ボタンの入力状態を監視(プレイヤー操作)
    //=====================================================================
    public bool PlayerRightArmsButton()
    {
        return false;
    }

    //=====================================================================
    //                背面左武器ボタンの入力状態を監視(プレイヤー操作)
    //=====================================================================
    public bool PlayerLeftBackArmsButton()
    {
        return false;
    }
    //=====================================================================
    //                背面右武器ボタンの入力状態を監視(プレイヤー操作)
    //=====================================================================
    public bool PlayerRightBackArmsButton()
    {
        return false;
    }


    //=====================================================================
    //                左右の入力値を取得(UI操作)
    //
    // 十字キー、左スティック両方の入力を取得する
    //=====================================================================
    public float UiCursorMoveHorizontal()
    {
        if (Input.GetAxis("L_Stick_H") != 0)
        {
            return Input.GetAxisRaw("L_Stick_H");

        }
        return Input.GetAxisRaw("D_Pad_H");
    }

  

    //=====================================================================
    //                上下の入力値を取得(UI操作)
    //
    // 十字キー、左スティック両方の入力を取得する
    //=====================================================================
    public float UiCursorMoveVertical()
    {
        if (Input.GetAxisRaw("L_Stick_V") != 0)
        {
            return Input.GetAxisRaw("L_Stick_V");

        }
        return Input.GetAxisRaw("D_Pad_V");
    }


    /*=====================================================================
    //            決定ボタンの入力状態を監視(UI操作)
    //=====================================================================*/
    public bool UiSelectButton()
    {
        return Input.GetButtonDown("A_button");
    }

    //=====================================================================
    //            キャンセルボタンの入力状態を監視(UI操作)
    //=====================================================================
    public bool UiCancelButton()
    {
        return Input.GetButtonDown("B_button");

    }  
    
    //=====================================================================
    //            ポーズボタンの入力状態を監視(UI操作)
    //=====================================================================
    public bool PoseButton()
    {
        return Input.GetButtonDown("Y_button");

    }               
    //=====================================================================
    //            Escやなどアプリケーションを閉じるボタン
    //=====================================================================
    public bool GameCloseButton()
    {
        //ゲームパッドには該当する処理なし
        return false;
    }               



    //=====================================================================
    //                異なる入力機器の入力を監視
    //=====================================================================
    public bool OtherInputCheak()
    {
        bool inputcheak = false;
        if (Input.GetButtonDown("Horizontal")) inputcheak = true;
        if (Input.GetButtonDown("Vertical")) inputcheak = true;
        if (Input.GetKeyDown(KeyCode.Space)) inputcheak = true;
        if( Input.GetKeyDown(KeyCode.Z))inputcheak = true;
        if (Input.GetKeyDown(KeyCode.X)) inputcheak = true;
        if (Input.GetKeyDown(KeyCode.P)) inputcheak = true;
        return inputcheak;
    }
}
