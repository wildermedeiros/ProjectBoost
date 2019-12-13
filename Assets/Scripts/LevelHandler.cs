using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (SceneManager.sceneCountInBuildSettings == nextSceneIndex)
        {
            nextSceneIndex = 0; // loop back to start 
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void LoadSavedScene()
    {
        PlayerData playerData = SaveAndLoadSystem.LoadData();

        SceneManager.LoadScene(playerData.level);
    }

    public void ReturnToMainMenu(int mainMenuIndex)
    {
        SceneManager.LoadScene(mainMenuIndex);
    }
}
