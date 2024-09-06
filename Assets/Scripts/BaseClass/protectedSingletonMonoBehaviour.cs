using System;
using UnityEngine;
using UnityEngine.Assertions;

//----------------------------------------------------------------------------
//                      �A�N�Z�X�������������V���O���g���p�^�[��(MonoBehaviour�p��)
//
// �u�B��̃C���X�^���X�v��ۏ؂��A���J�͈͂�h���N���X�܂łɌ��肵�����N���X
// �}�l�[�W���[�N���X��MonoBehaviour���p������(�I�u�W�F�N�g�����������)�N���X�Ƃ��Ďg�����Ƃ�z��
//
// �� DontDestroyOnLoad��Awake���\�b�h���g���Ă��邽�߁A�h���N���X�Ő錾���Ȃ��悤�ɒ���!!
//----------------------------------------------------------------------------


public abstract class protectedSingletonMonoBehaviour<TYpe> : MonoBehaviour, IDisposable where TYpe : MonoBehaviour
{
    private static  TYpe instance;     //�C���X�^���X

    protected static TYpe Instance      //���J����C���X�^���X(�h���N���X�܂ł̂݃A�N�Z�X�\)
    {
        get
        {
            Assert.IsNotNull(instance, "There is no object attached " + typeof(TYpe).Name);
            return instance;
        }
    }

    // ���݃`�F�b�N(True:����, False:�C���X�^���X������)
    public static bool IsExist() { return instance != null; }

    //======================================================
    //                  ������
    //
    // �C���X�^���X�̃`�F�b�N��GameObject�̕ی�
    // 1.�C���X�^���X�̃`�F�b�N�B�C���X�^���X��������Δj������
    // 2.�N���X�̃C���X�^���X�̐���
    // 3.DontDestroyOnLoad�̐ݒ�
    // 4.�ǉ��ŏ��������\�b�h�����s����
    //======================================================

    protected virtual void Awake()
    {
        // 1.�C���X�^���X�̃`�F�b�N�B�C���X�^���X��������Δj������
        if (instance != null && instance != this)
        {
            Debug.Log($"�C���X�^���X�����ς݁B�V���ɍ��ꂽ������{this.gameObject.name}��j�����܂�");
            Destroy(this.gameObject);  // �V�����C���X�^���X��j������
            return;
        }
        
        // 2.�N���X�̃C���X�^���X�̐���
        instance = this as TYpe;

        // 3.DontDestroyOnLoad�̐ݒ�
        DontDestroyOnLoad(this.gameObject);  // �V�[�����ׂ��ł��폜����Ȃ��悤�ɂ���

        // 4.�ǉ��ŏ��������\�b�h�����s����
        OnAwakeProcess();  
    }

    //======================================================
    //                  ���������̒ǉ�����
    //
    // �h����ł��������������������߂�API
    //======================================================
    protected virtual void OnAwakeProcess()
    {
        //�h���N���X�ŋL�� & �����ł͏����Ȃ�!
    }

    //======================================================
    //          �I�u�W�F�N�g�j�����̏���
    //======================================================
    private void OnDestroy()
    {
        // ���g�ȊO�̃C���X�^���X���쐬�������j�������Ƃ��ɊԈ���Ď��s����Ȃ��悤�Ƀu���b�N
        if (instance != (this as TYpe)) return;
        OnDestroyProcess();
        Dispose();
    }

    //======================================================
    //          �I�u�W�F�N�g�̔j�����̒ǉ�����
    //
    // / �h����ł��������������������߂�API
    //======================================================
    // �h����ł��j���������������߂�API
    protected virtual void OnDestroyProcess()
    {
        //�h���N���X�ŋL�� & �����ł͏����Ȃ�!
    }

    //======================================================
    //                 �j�����̏���
    //======================================================
    public virtual void Dispose()
    {
        // ����instance������Ȃ炻���j������
        if (IsExist()) instance = null;
    }
}