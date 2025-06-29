using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);  
    }

    public void loadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}//LoadSceneManager
