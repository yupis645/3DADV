
//--------------------------------------------------------------------------------------------------------
//                        ���̓N���X(�}�l�[�W���[�N���X)
//
// �T�v : IInputProvider�C���^�[�t�F�[�X���p�����������N���X�B
// ���� : ���͋@�킩��̓��͂� bool ������ float �Ŏ擾����B
//
// ���݂̓L�[�{�[�h�A�Q�[���p�b�h(xbox�R���g���[���[)�̓��؂�ւ��Ďg�����Ƃ��ł���
//--------------------------------------------------------------------------------------------------------

public class InputManager : protectedSingletonMonoBehaviour<InputManager>, IInputManager
{
    //*************************************************************************************************************************
    //                                          ���\�b�h

    //===========================================================================================
    //                      ���������\�b�h
    //===========================================================================================
    public int Initialize()
    {
        //InstanceManager.Instance.SetInputProviderInstance<KeyboardInputProvider>();

        return 0;
    }

    //===========================================================================================
    //                      �����ݒ胁�\�b�h
    //===========================================================================================
    public int SetUp()
    {

        return 0;
    }



    //===========================================================================================
    //                      ���j�����\�b�h
    //
    // ���N���X��Ondestroy���\�b�h�̒��ɂ�����s���\�b�h�B
    //===========================================================================================
    // �h����ł��j���������������߂�API
    protected override void OnDestroyProcess()
    {
    }


    //=====================================================================
    //                ���E�̓��͒l���擾(�v���C���[�I�u�W�F�N�g����)
    //=====================================================================
    public float PlayerMoveHorizontal()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerMoveHorizontal();

    }

    //=====================================================================
    //                �㉺�̓��͒l���擾(�v���C���[�I�u�W�F�N�g����)
    //=====================================================================
    public float PlayerMoveVertical()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerMoveVertical();

    }

    //=====================================================================
    //                �W�����v�̓��͂��擾(�v���C���[�I�u�W�F�N�g����)
    //=====================================================================
    public bool PlayerJumpButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerJumpButton();

    }
    //=====================================================================
    //                �_�b�V���{�^���̓��͏�Ԃ��Ď�(�v���C���[����)
    //=====================================================================
    public bool PlayerDashButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerDashButton();
    }
    //=====================================================================
    //                ���r����{�^���̓��͏�Ԃ��Ď�(�v���C���[����)
    //=====================================================================
    public bool PlayerLeftArmsButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerLeftArmsButton();
    }
    //�v���C���[�̍��r�̕���g�p
    //=====================================================================
    //                �E�r����{�^���̓��͏�Ԃ��Ď�(�v���C���[����)
    //=====================================================================
    public bool PlayerRightArmsButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerRightArmsButton();
    }

    //=====================================================================
    //                �w�ʍ�����{�^���̓��͏�Ԃ��Ď�(�v���C���[����)
    //=====================================================================
    public bool PlayerLeftBackArmsButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerLeftBackArmsButton();
    }
    //=====================================================================
    //                �w�ʉE����{�^���̓��͏�Ԃ��Ď�(�v���C���[����)
    //=====================================================================
    public bool PlayerRightBackArmsButton()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().PlayerRightBackArmsButton();
    }



    //=====================================================================
    //                ���E�̓��͒l���擾(UI����)
    //=====================================================================
    public float UiCursorMoveHorizontal()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().UiCursorMoveHorizontal();

    }

    //=====================================================================
    //                ���E�̓��͏�Ԃ��擾(UI����)
    //=====================================================================
    public bool HorizontalInputDown()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().HorizontalInputDown();
      
    }

    //=====================================================================
    //               �㉺�̓��͒l���擾(UI����)
    //=====================================================================
    public float UiCursorMoveVertical()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().UiCursorMoveVertical();

    }

    //=====================================================================
    //                �㉺�̓��͏�Ԃ��擾(UI����)
    //=====================================================================
    public bool VerticalInputDown()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().VerticalInputDown();
    }

    //=====================================================================
    //                ����{�^���̓��͏�Ԃ��擾(UI����)
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
    //                �L�����Z���{�^���̓��͏�Ԃ��擾(UI����)
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
    //                �|�[�Y�{�^���̓��͏�Ԃ��擾(UI����)
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
    //                ���͏�Ԃ̊Ď�
    //
    // ���݂̓��̓��[�h�Ƃ͈قȂ���͋@�킩��̓��͂��擾����B
    // �L�[�{�[�h���[�h�Ȃ�Q�[���p�b�h�̓��͂��Ď�
    // �Q�[���p�b�g�Ȃ�L�[�{�[�h�̓��͂��Ď�
    //=====================================================================
    public bool OtherInputCheak()
    {
        return InstanceManager.Instance.GetInputProviderInstance<IInputProvider>().OtherInputCheak();
    }                




}
