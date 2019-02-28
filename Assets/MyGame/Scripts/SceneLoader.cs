using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    enum Scenes {Intro, Main, GameOver};

    public void LoadIntroScene()
    {
        SceneManager.LoadScene((int)Scenes.Intro);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene((int)Scenes.Main);
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene((int)Scenes.GameOver);
    }

}
