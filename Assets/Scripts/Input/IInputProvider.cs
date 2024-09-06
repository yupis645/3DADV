
//--------------------------------------------------------------------------------------------------------
//                        ���͂̃C���^�[�t�F�[�X
//--------------------------------------------------------------------------------------------------------

public interface IInputProvider
{
    bool HorizontalInputDown();
    bool VerticalInputDown();

    //�v���C���[�̓���
    float PlayerMoveHorizontal();       //�v���C���[�̍��E�ړ�
    float PlayerMoveVertical();         //�v���C���[�̑O��ړ�
    bool PlayerJumpButton();            //�v���C���[�̃W�����v
    bool PlayerDashButton();            //�v���C���[�̃_�b�V��
    bool PlayerLeftArmsButton();            //�v���C���[�̍��r�̕���g�p
    bool PlayerRightArmsButton();            //�v���C���[�̉E�r�̕���g�p
    bool PlayerLeftBackArmsButton();            //�v���C���[�̔w�ʍ��̕���g�p
    bool PlayerRightBackArmsButton();            //�v���C���[�̔w�ʉE�̕���g�p

    //Ui�̓���
    float UiCursorMoveHorizontal();     //�J�[�\���̏㉺�ړ�
    float UiCursorMoveVertical();       //�J�[�\���̍��E�ړ�

    bool UiSelectButton();              //����{�^��
    bool UiCancelButton();              //�L�����Z���{�^��
    bool PoseButton();                  //�|�[�Y�{�^��

    bool OtherInputCheak();             //���͂̊Ď����\�b�h

    bool GameCloseButton();

}

