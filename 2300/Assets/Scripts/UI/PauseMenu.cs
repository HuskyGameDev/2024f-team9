using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public static bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);   
        settingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }
        
    }

    public void PauseGame(){
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }
    
    public void ResumeGame(){
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        isPaused = false;
    }

    public void GoToSettingsMenu(){
        Time.timeScale = 0f;
        pauseMenu.SetActive(false); // Hide the pause menu
        settingsMenu.SetActive(true); // Show the settings menu
    }

    public void BackToPauseMenu()
    {
        settingsMenu.SetActive(false); // Hide the settings menu
        pauseMenu.SetActive(true); // Show the pause menu
    }
}
