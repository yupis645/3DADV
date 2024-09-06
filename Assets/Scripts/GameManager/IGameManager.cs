using UnityEngine.SceneManagement;

public interface IGameManager
{
    int Initialize();
    int SetUp();
    void GameUpdate();
    void OnSceneLoadEvent(string scenename);
    void OnSceneUnloadEvent(Scene thisScene);
    void SceneChange(int NextScene);
    void QuitGame();

}
