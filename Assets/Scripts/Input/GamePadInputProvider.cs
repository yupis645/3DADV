using UnityEngine;

//--------------------------------------------------------------------------------------------------------
//                        �Q�[���p�b�h�̓��͎����N���X
//
// IInputProvider�C���^�[�t�F�[�X���p���������̓N���X�B
// �Q�[���p�b�g����̓��͂��󂯎��
//--------------------------------------------------------------------------------------------------------

public class GamePadInputProvider : IInputProvider
{
    static bool hori = false;
    static bool ver = false;

    //=====================================================================
    //                ���E�̓��͏�Ԃ��Ď�(UI����)
    //
    // �\���L�[�A���X�e�B�b�N�����̓��͂����Ď�����
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
    //                �㉺�̓��͏�Ԃ��Ď�(UI����)
    //
    // �\���L�[�A���X�e�B�b�N�����̓��͂����Ď�����
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
    //                ���E�̓��͒l���擾(�v���C���[����)
    //
    // �\���L�[�A���X�e�B�b�N�����̓��͂��擾����
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
    //                �㉺�̓��͒l���擾(�v���C���[����)
    //
    // �\���L�[�A���X�e�B�b�N�����̓��͂��擾����
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
    //                �W�����v�{�^���̓��͏�Ԃ��Ď�(�v���C���[����)
    //=====================================================================
    public bool PlayerJumpButton()
    {
        return Input.GetButtonDown("A_button");
    }            //�v���C���[�̃W�����v


    //=====================================================================
    //                �_�b�V���{�^���̓��͏�Ԃ��Ď�(�v���C���[����)
    //=====================================================================
    public bool PlayerDashButton()
    {

        return true;
    }
    //=====================================================================
    //                ���r����{�^���̓��͏�Ԃ��Ď�(�v���C���[����)
    //=====================================================================
    public bool PlayerLeftArmsButton()
    {
        return false;
    }
    //�v���C���[�̍��r�̕���g�p
    //=====================================================================
    //                �E�r����{�^���̓��͏�Ԃ��Ď�(�v���C���[����)
    //=====================================================================
    public bool PlayerRightArmsButton()
    {
        return false;
    }

    //=====================================================================
    //                �w�ʍ�����{�^���̓��͏�Ԃ��Ď�(�v���C���[����)
    //=====================================================================
    public bool PlayerLeftBackArmsButton()
    {
        return false;
    }
    //=====================================================================
    //                �w�ʉE����{�^���̓��͏�Ԃ��Ď�(�v���C���[����)
    //=====================================================================
    public bool PlayerRightBackArmsButton()
    {
        return false;
    }


    //=====================================================================
    //                ���E�̓��͒l���擾(UI����)
    //
    // �\���L�[�A���X�e�B�b�N�����̓��͂��擾����
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
    //                �㉺�̓��͒l���擾(UI����)
    //
    // �\���L�[�A���X�e�B�b�N�����̓��͂��擾����
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
    //            ����{�^���̓��͏�Ԃ��Ď�(UI����)
    //=====================================================================*/
    public bool UiSelectButton()
    {
        return Input.GetButtonDown("A_button");
    }

    //=====================================================================
    //            �L�����Z���{�^���̓��͏�Ԃ��Ď�(UI����)
    //=====================================================================
    public bool UiCancelButton()
    {
        return Input.GetButtonDown("B_button");

    }  
    
    //=====================================================================
    //            �|�[�Y�{�^���̓��͏�Ԃ��Ď�(UI����)
    //=====================================================================
    public bool PoseButton()
    {
        return Input.GetButtonDown("Y_button");

    }               
    //=====================================================================
    //            Esc��ȂǃA�v���P�[�V���������{�^��
    //=====================================================================
    public bool GameCloseButton()
    {
        //�Q�[���p�b�h�ɂ͊Y�����鏈���Ȃ�
        return false;
    }               



    //=====================================================================
    //                �قȂ���͋@��̓��͂��Ď�
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
