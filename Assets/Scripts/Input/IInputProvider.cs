
//--------------------------------------------------------------------------------------------------------
//                        入力のインターフェース
//--------------------------------------------------------------------------------------------------------

public interface IInputProvider
{
    bool HorizontalInputDown();
    bool VerticalInputDown();

    //プレイヤーの入力
    float PlayerMoveHorizontal();       //プレイヤーの左右移動
    float PlayerMoveVertical();         //プレイヤーの前後移動
    bool PlayerJumpButton();            //プレイヤーのジャンプ
    bool PlayerDashButton();            //プレイヤーのダッシュ
    bool PlayerLeftArmsButton();            //プレイヤーの左腕の武器使用
    bool PlayerRightArmsButton();            //プレイヤーの右腕の武器使用
    bool PlayerLeftBackArmsButton();            //プレイヤーの背面左の武器使用
    bool PlayerRightBackArmsButton();            //プレイヤーの背面右の武器使用

    //Uiの入力
    float UiCursorMoveHorizontal();     //カーソルの上下移動
    float UiCursorMoveVertical();       //カーソルの左右移動

    bool UiSelectButton();              //決定ボタン
    bool UiCancelButton();              //キャンセルボタン
    bool PoseButton();                  //ポーズボタン

    bool OtherInputCheak();             //入力の監視メソッド

    bool GameCloseButton();

}

