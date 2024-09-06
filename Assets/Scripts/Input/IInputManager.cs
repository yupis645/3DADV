
//--------------------------------------------------------------------------------------------------------
//                        入力マネージャーのインターフェース
//--------------------------------------------------------------------------------------------------------
public interface IInputManager
{
    public int Initialize();            
    public int SetUp();
    public bool HorizontalInputDown();
    public bool VerticalInputDown();

    public float PlayerMoveHorizontal();
    public float PlayerMoveVertical();

    public bool PlayerJumpButton();

    public bool PlayerDashButton();            //プレイヤーのダッシュ
    public bool PlayerLeftArmsButton();            //プレイヤーの左腕の武器使用
    public bool PlayerRightArmsButton();            //プレイヤーの右腕の武器使用
    public bool PlayerLeftBackArmsButton();            //プレイヤーの背面左の武器使用
    public bool PlayerRightBackArmsButton();            //プレイヤーの背面右の武器使用

    public float UiCursorMoveHorizontal();
    public float UiCursorMoveVertical();
    public bool UiSelectButton();

    public bool UiCancelButton();
  
    public bool PoseButton();

    bool GameCloseButton();
    public bool OtherInputCheak();



}
