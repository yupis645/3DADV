using UnityEngine;

//------------------------------------------------------------------------------------------------------------------------------------------
//                    �V�[���S�̂̃C���X�^���X���Ǘ�����}�l�[�W���[�N���X(�V���O���g��)
//
// �}�l�[�W���[�N���X�Ȃǂ̃V�[���S�̂ŋ��L����C���X�^���X��DI�R���e�i�֓o�^�A�Ăяo���A�C���X�^���X�̃Z�b�g�Ȃǂ�����
// �܂��Aprefub��e�L�X�g�f�[�^�Ȃǂ��i�[���Ă���A�f�[�^���擾����̂ɂ͂��̃N���X���o�R����K�v������B
// �����N���X�ŃV���O���g���̎��������Ă���AAwake���g�p���Ă���̂ł����ł͍Ē�`���Ȃ����ƁB
//------------------------------------------------------------------------------------------------------------------------------------------

public class InstanceManager : SingletonMonoBehaviour<InstanceManager>
{
    //****************************************************************************************************************************
    //                                          �ϐ��錾
    
    //�}�l�[�W���[�N���X��prefub
    [SerializeField] GameObject GameManagerPrefub;  
    [SerializeField] GameObject InputManagerPrefub;


    //DI�R���e�i�̃C���X�^���X
    private DIContainer container = new DIContainer();

    //�Q�[���S�̂ŋ��L����f�[�^�Q�̃C���X�^���X�擾(inspector���擾)
    [SerializeField]public MasterData _masterData;    //�S�V�[���ŋ��L����f�[�^�N���X

    //[SerializeField] SoundData _soundData;

    // �O���Ɍ��J����f�[�^�Q��getter��`
    public MasterData masterData { get => _masterData; }

    //*******************************************************************************************************
    //                          ���\�b�h

    //=====================================================================
    //                  ������
    //
    // ���N���X��Awake�N���X�ɂ���ǉ����\�b�h�ɏ�������ŏ���������B
    // 1.�}�l�[�W���[�N���X�̃C���X�^���X��
    // 2.�쐬�����}�l�[�W���[�N���X�̃C���X�^���X��DI�R���e�i�ɓo�^
    // 3.���͋@��(�L�[�{�[�h��Q�[���p�b�h)�̓��̓C���^�[�t�F�[�X��o�^
    // 4.�f�[�^�N���X���C���X�^���X������
    //=====================================================================
    protected override void OnAwakeProcess()
    {
        // 1.�}�l�[�W���[�N���X�̃C���X�^���X��
        GameObject im = Instantiate(InputManagerPrefub);    //�C���v�b�g�}�l-�W���[
        GameObject gm = Instantiate(GameManagerPrefub);     //�Q�[���}�l�[�W���[

        // 2.�쐬�����}�l�[�W���[�N���X�̃C���X�^���X��DI�R���e�i�ɓo�^
        container.Register<InstanceManager>(() => this);                                //����(�C���X�^���X�}�l�[�W���[)
        container.Register<IInputManager>(() => im.GetComponent<InputManager>());       //�C���v�b�g�}�l�[�W��-
        container.Register<IGameManager>(() => gm.GetComponent<GameManager>());         //�Q�[���}�l�[�W���[


        // 3.���͋@��(�L�[�{�[�h��Q�[���p�b�h)�̓��̓C���^�[�t�F�[�X��o�^
        container.Register<KeyboardInputProvider>(() => new KeyboardInputProvider());       //�L�[�{�[�h�p
        container.Register<GamePadInputProvider>(() => new GamePadInputProvider());         //�Q�[���p�b�h�p

        // 4.�f�[�^�N���X���C���X�^���X������
        _masterData = new MasterData();

    }

    //=====================================================================
    //                  �����ݒ�
    //
    // �����ւ��\�ȃC���X�^���X�Ȃǂ̏�����Ԃɂ���
    // 1.���͋@����L�[�{�[�h�������ݒ�ɂ���
    //=====================================================================
    private void Start()
    {
        // 1.���͋@����L�[�{�[�h�������ݒ�ɂ���
        container.Register<IInputProvider>(() => GetInputProviderInstance<KeyboardInputProvider>());         //���͋@��̏����ݒ�
   
        // 2.�Q�[���}�l�[�W���[�̏������n�߂�B(�Q�[���̊J�n)
        GetGameManagerInstance<IGameManager>().Initialize();
        GetGameManagerInstance<IGameManager>().SetUp();

    }

    //=====================================================================
    //                  ���͋@��̓���ւ�
    //
    // ���݂̓��̓��[�h����ʂ̓��͋@��ɕύX����B
    //====================================================================
    public void ChangeInputMode()
    {

        // 1.���݂̓��̓��[�h���L�[�{�[�h���[�h�Ȃ�
        if (GetInputProviderInstance<IInputProvider>() is KeyboardInputProvider)
        {
            container.SetInstance<IInputProvider>(GetInputProviderInstance<GamePadInputProvider>());
        }

        // 2.���݂̓��̓��[�h���Q�[���p�b�h���[�h�Ȃ�
        else if (GetInputProviderInstance<IInputProvider>() is GamePadInputProvider)
        {
            container.SetInstance<IInputProvider>(GetInputProviderInstance<KeyboardInputProvider>());
        }
    }

    
    //***********************************************************************************************************************
    //                              DI�R���e�i����̎擾���\�b�h


    public T GetGameManagerInstance<T>() where T : class,IGameManager
    {
        return container.Resolve<T>();
    }
    public T GetInstanceManagerInstance<T>() where T : InstanceManager
    {
        return container.Resolve<T>();
    }

    public T GetInputManagerInstance<T>() where T : class,IInputManager
    {
        return container.Resolve<T>();
    }

     public T GetInputProviderInstance<T>() where T : class,IInputProvider
    {
        return container.Resolve<T>();
    }




}


