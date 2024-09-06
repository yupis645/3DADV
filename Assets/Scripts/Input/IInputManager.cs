
//--------------------------------------------------------------------------------------------------------
//                        ���̓}�l�[�W���[�̃C���^�[�t�F�[�X
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

    public bool PlayerDashButton();            //�v���C���[�̃_�b�V��
    public bool PlayerLeftArmsButton();            //�v���C���[�̍��r�̕���g�p
    public bool PlayerRightArmsButton();            //�v���C���[�̉E�r�̕���g�p
    public bool PlayerLeftBackArmsButton();            //�v���C���[�̔w�ʍ��̕���g�p
    public bool PlayerRightBackArmsButton();            //�v���C���[�̔w�ʉE�̕���g�p

    public float UiCursorMoveHorizontal();
    public float UiCursorMoveVertical();
    public bool UiSelectButton();

    public bool UiCancelButton();
  
    public bool PoseButton();

    bool GameCloseButton();
    public bool OtherInputCheak();



}
