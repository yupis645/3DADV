
using UnityEngine;
using UnityEngine.SceneManagement;

//--------------------------------------------------------------------------------------------------------
//                       ゲーム全体の管理クラス
//
// ゲーム進行を管理するクラス。各インスタンスの初期化や更新などはここで行う。
// 各ステートは
//  ・StartUpステート      : GameManagerのインスタンスなどを生成する。ゲーム開始時に一瞬だけ経由して次のシーンへ移行する
//  ・Titleステート        : タイトル画面。
//  ・StageSelectステート  : 遊ぶステージを選択する
//  ・Gameステート         : プレイヤーを操作してゲームをする
//--------------------------------------------------------------------------------------------------------


public class GameManager : protectedSingletonMonoBehaviour<GameManager>, IGameManager
{
    //********************************************************************************************************************
    //                                  変数宣言

    //ステートマシン
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

        // 2.ゲームモードをタイトルにする。
        Game = State.Title;

        // 3.シーンをロードした時のイベントを追加。
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 4.シーンをアンロードした時のイベントを追加。
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        // 5.現在のシーンが"StartUp"だった場合、Titleシーンへ移行する
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
                // タイトルシーンの読み込み
                SceneManager.LoadScene("Title");
                Game = State.Title;
                break;
            case Constants.STAGESELECT:
                // タイトルシーンの読み込み(StateをStageselcetに変更)
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
    //                ゲームの終了
    //
    // エディター及びアプリケーションを終了させる
    //======================================================================
    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }

    }

