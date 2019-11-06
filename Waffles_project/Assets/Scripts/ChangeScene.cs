using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class handles scene changing & quitting application
 */

public class ChangeScene : MonoBehaviour
{
    public void ChangeMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
   public void QuitGame()
    {
        Application.Quit();
    }
}
