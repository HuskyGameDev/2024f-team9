using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void GoToSettingsMenu(){
        SceneManager.LoadScene("SettingsMenu");
    }

    public void GoToMainMenu(){
        SceneManager.LoadScene(0);
    }

    public void SetFullscreen (bool isFullScreen){
        Screen.fullScreen = isFullScreen;
    }

    public void QuitGame(){
        Application.Quit();
    }
}
