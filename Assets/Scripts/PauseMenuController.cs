using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;      
    public GameObject controlsPanel; 
    public GameObject controlsMenuStart;
    private bool isPaused = false;   
    public GameObject startMenu;     

    private void Start()
    {
        startMenu.SetActive(true);  
        pauseMenu.SetActive(false); 
        Time.timeScale = 0f;        
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape) && !startMenu.activeSelf && !controlsMenuStart.activeSelf)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

   
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;          
        pauseMenu.SetActive(true);   
    }

   
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;         
        pauseMenu.SetActive(false); 
    }

   
    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    
    public void ShowControls()
    {
        pauseMenu.SetActive(false);   
        controlsPanel.SetActive(true); 
    }

    
    public void BackToPauseMenu()
    {
        controlsPanel.SetActive(false); 
        pauseMenu.SetActive(true);      
    }

    
    public void PlayGame()
    {
        startMenu.SetActive(false); 
        Time.timeScale = 1f;         
    }

    
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;  
    }
    public void BackToStartMenu()
    {
        controlsMenuStart.SetActive(false); 
        startMenu.SetActive(true);      
    }
    public void ShowControlsStart()
    {
        startMenu.SetActive(false);   
        controlsMenuStart.SetActive(true); 
    }
}
