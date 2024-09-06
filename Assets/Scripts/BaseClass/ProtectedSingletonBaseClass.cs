using System;
using UnityEngine.Assertions;

//----------------------------------------------------------------------------
//                      �A�N�Z�X�������������V���O���g���p�^�[��(MonoBehaviour���p�����Ȃ�)
//
// �u�B��̃C���X�^���X�v��ۏ؂��A���J�͈͂�h���N���X�܂łɌ��肵�����N���X
// 
//----------------------------------------------------------------------------


public abstract class ProtectedSingletonBaseClass<T> : IDisposable where T : class, new()
{
    private static T instance;       //�ێ�����C���X�^���X

    protected static T Instance      //���J����C���X�^���X(�h���N���X�܂ł̂݃A�N�Z�X�\)
    {
        get
        {
            Assert.IsNotNull(instance, "There is no object attached " + typeof(T).Name);
            return instance;
        }
    }

    protected static bool IsExist() => instance != null;

    //======================================================
    //                  ������
    //
    // �C���X�^���X�̃`�F�b�N��GameObject�̕ی�
    // 1.�C���X�^���X�̃`�F�b�N�B�C���X�^���X��������Δj������
    // 2.�N���X�̃C���X�^���X�̐���
    // 3.DontDestroyOnLoad�̐ݒ�
    // 4.�ǉ��ŏ��������\�b�h�����s����
    //======================================================
    protected ProtectedSingletonBaseClass()
    {
        if (instance != null && instance != this)
        {
            return;
        }
        instance = this as T;

        CustomConstructor();  // �h����̏���������
    }
    //======================================================
    //                  ���������̒ǉ�����
    //
    // �h����ł��������������������߂�API
    //======================================================
    protected virtual void CustomConstructor()
    {

    }

    // Destroy������

    //======================================================
    //          �I�u�W�F�N�g�j�����̏���
    //======================================================
    public virtual void Dispose()
    {
        OnDestroyProcess();
        instance = null;
    }


    //======================================================
    //                  ���������̒ǉ�����
    //
    // �h����ł��������������������߂�API
    //======================================================
    protected virtual void OnDestroyProcess()
    {
    }
}