
using System;
using UnityEngine;
using UnityEngine.Assertions;

//-------------------------------------------------------------------------------------------------------
//                          MonoBehaviour�p������Singleton
//-------------------------------------------------------------------------------------------------------
public abstract class SingletonMonoBehaviour<TYpe> : MonoBehaviour, IDisposable where TYpe : MonoBehaviour
{
    private static TYpe instance;

    public static TYpe Instance
    {
        get
        {
            Assert.IsNotNull(instance, "There is no object attached " + typeof(TYpe).Name);
            return instance;
        }
    }

    //���݃`�F�b�N(True:����, False:�C���X�^���X������)
    public static bool IsExist() { return instance != null; }

    private void Awake()
    {
        if (instance != null && instance.gameObject != null)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);        //�V�[�����ׂ��ł��폜����Ȃ��悤�ɂ���

        instance = this as TYpe;        //�C���X�^���X���쐬����

        OnAwakeProcess();   //�h����̏���������
    }

    //====================================================================================
    //              �h����ł��������������������߂�API
    //====================================================================================
    protected virtual void OnAwakeProcess() 
    {

    }

    //====================================================================================
    ///                     Destroy������
    ///
    //         �h����Ŏ��s�R�ꂪ�����悤�ɈӐ}�I��Private
    //====================================================================================
    private void OnDestroy()
    {
        // ���g�ȊO�̃C���X�^���X���쐬�������j�������Ƃ��ɊԈ���Ď��s����Ȃ��悤�Ƀu���b�N
        if (instance != (this as TYpe)) return;
        OnDestroyProcess();
        Dispose();
    }

    //====================================================================================
    ///                     �h����ł��j���������������߂�API
    //====================================================================================
    protected virtual void OnDestroyProcess()
    {
        //�������z�֐��Ȃ̂ł����ł͏����������Ȃ�����!!  
    }

    //====================================================================================
    ///                     ����instance������Ȃ炻���j������
    //====================================================================================
    public virtual void Dispose()
    {
        if (IsExist()) instance = null;
    }

}
