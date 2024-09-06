
using UnityEngine;
using UnityEngine.SceneManagement;

//--------------------------------------------------------------------------------------------------------
//                       �Q�[���S�̂̊Ǘ��N���X
//
// �Q�[���i�s���Ǘ�����N���X�B�e�C���X�^���X�̏�������X�V�Ȃǂ͂����ōs���B
// �e�X�e�[�g��
//  �EStartUp�X�e�[�g      : GameManager�̃C���X�^���X�Ȃǂ𐶐�����B�Q�[���J�n���Ɉ�u�����o�R���Ď��̃V�[���ֈڍs����
//  �ETitle�X�e�[�g        : �^�C�g����ʁB
//  �EStageSelect�X�e�[�g  : �V�ԃX�e�[�W��I������
//  �EGame�X�e�[�g         : �v���C���[�𑀍삵�ăQ�[��������
//--------------------------------------------------------------------------------------------------------


public class GameManager : protectedSingletonMonoBehaviour<GameManager>, IGameManager
{
    //********************************************************************************************************************
    //                                  �ϐ��錾

    //�X�e�[�g�}�V��
    public enum State
    {
        StartUp,
        Title,
        StageSelect,
        Game,
    }

    public State Game { get; private set; }

    public State GetGameState()
    {
        return Game;
    }

    public int Initialize()
    {
        int im = InstanceManager.Instance.GetInputManagerInstance<IInputManager>().Initialize();

        return 0;
    }
    public int SetUp()
    {

        int im = InstanceManager.Instance.GetInputManagerInstance<IInputManager>().SetUp();
        if (im == -1) QuitGame();

        // 2.�Q�[�����[�h���^�C�g���ɂ���B
        Game = State.Title;

        // 3.�V�[�������[�h�������̃C�x���g��ǉ��B
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 4.�V�[�����A�����[�h�������̃C�x���g��ǉ��B
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        // 5.���݂̃V�[����"StartUp"�������ꍇ�ATitle�V�[���ֈڍs����
        if (SceneManager.GetActiveScene().name == "StartUp")
        {
            SceneChange(Constants.TITLE);
        }


        return 0;
    }


    private void Update()
    {
        GameUpdate();
    }
    public void GameUpdate()
    {
    

    }
    public void OnSceneLoaded(Scene nextScene, LoadSceneMode mode)
    {

        OnSceneLoadEvent(nextScene.name);

    }
    public void OnSceneLoadEvent(string scenename)
    {
    
    }
    public void OnSceneUnloaded(Scene thisScene)
    {
        OnSceneUnloadEvent(thisScene);

    }

    public void OnSceneUnloadEvent(Scene thisScene)
    {
      
    }
    public void SceneChange(int NextScene)
    {
        switch (NextScene)
        {
            case Constants.TITLE:
                // �^�C�g���V�[���̓ǂݍ���
                SceneManager.LoadScene("Title");
                Game = State.Title;
                break;
            case Constants.STAGESELECT:
                // �^�C�g���V�[���̓ǂݍ���(State��Stageselcet�ɕύX)
                SceneManager.LoadScene("Title");
                Game = State.StageSelect;
                break;
            case Constants.GAME:
                SceneManager.LoadScene("Game");
                break;
        }
    }

    void OnApplicationQuit()
    {
       

        Time.timeScale = 0;
    }

    //======================================================================
    //                �Q�[���̏I��
    //
    // �G�f�B�^�[�y�уA�v���P�[�V�������I��������
    //======================================================================
    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
        }

    }

