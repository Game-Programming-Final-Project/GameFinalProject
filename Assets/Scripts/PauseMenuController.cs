using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;      // Pause menu paneli
    public GameObject controlsPanel; // Controls paneli
    public GameObject controlsMenuStart;
    private bool isPaused = false;   // Oyunun duraklat�l�p duraklat�lmad���n� takip eder
    public GameObject startMenu;     // Start menu paneli

    private void Start()
    {
        startMenu.SetActive(true);  // Ba�lang�� men�s�n� g�ster
        pauseMenu.SetActive(false); // Pause men�s�n� gizle
        Time.timeScale = 0f;        // Oyun duraklat�l�r (Ba�lang��ta oyun duraklat�lmal�)
    }

    void Update()
    {
        // ESC tu�una bas�ld���nda duraklatma men�s�n� a�/kapat, ama ba�lang�� men�s�nde iken engelle
        if (Input.GetKeyDown(KeyCode.Escape) && !startMenu.activeSelf && !controlsMenuStart.activeSelf)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // Oyunu duraklat ve pause men�s�n� g�ster
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;          // Oyun duraklat�l�r
        pauseMenu.SetActive(true);   // Pause men�s� a��l�r
    }

    // Oyunu devam ettir ve pause men�s�n� kapat
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;          // Oyun devam eder
        pauseMenu.SetActive(false);  // Pause men�s� kapan�r
    }

    // Oyunu yeniden ba�lat
    public void RestartGame()
    {
        Time.timeScale = 1f; // Oyun h�z�n� normale d�nd�r
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Mevcut sahneyi yeniden y�kle
    }

    // Kontroller panelini g�ster
    public void ShowControls()
    {
        pauseMenu.SetActive(false);   // Pause men�s�n� kapat
        controlsPanel.SetActive(true); // Kontroller panelini a�
    }

    // Kontrollerden pause men�s�ne d�n
    public void BackToPauseMenu()
    {
        controlsPanel.SetActive(false); // Kontroller panelini kapat
        pauseMenu.SetActive(true);      // Pause men�s�n� a�
    }

    // Oyun ba�latma (Start Menu'den Play t�klan�rsa)
    public void PlayGame()
    {
        startMenu.SetActive(false);  // Start menu'yi gizle
        Time.timeScale = 1f;         // Oyun ba�lar
    }

    // Oyundan ��kma (Start Menu'den Exit t�klan�rsa)
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;  // Oyunu kapat
    }
    public void BackToStartMenu()
    {
        controlsMenuStart.SetActive(false); // Kontroller panelini kapat
        startMenu.SetActive(true);      // Pause men�s�n� a�
    }
    public void ShowControlsStart()
    {
        startMenu.SetActive(false);   // Pause men�s�n� kapat
        controlsMenuStart.SetActive(true); // Kontroller panelini a�
    }
}
