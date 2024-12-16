using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;      // Pause menu paneli
    public GameObject controlsPanel; // Controls paneli
    private bool isPaused = false;   // Oyunun duraklatýlýp duraklatýlmadýðýný takip eder

    void Update()
    {
        // ESC tuþuna basýldýðýnda duraklatma menüsünü aç/kapat
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // Oyunu duraklat ve pause menüsünü göster
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;          // Oyun duraklatýlýr
        pauseMenu.SetActive(true);   // Pause menüsü açýlýr
    }

    // Oyunu devam ettir ve pause menüsünü kapat
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;          // Oyun devam eder
        pauseMenu.SetActive(false); // Pause menüsü kapanýr
    }

    // Oyunu yeniden baþlat
    public void RestartGame()
    {
        Time.timeScale = 1f; // Oyun hýzýný normale döndür
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Mevcut sahneyi yeniden yükle
    }

    // Kontroller panelini göster
    public void ShowControls()
    {
        pauseMenu.SetActive(false);   // Pause menüsünü kapat
        controlsPanel.SetActive(true); // Kontroller panelini aç
    }

    // Kontrollerden pause menüsüne dön
    public void BackToPauseMenu()
    {
        controlsPanel.SetActive(false); // Kontroller panelini kapat
        pauseMenu.SetActive(true);     // Pause menüsünü aç
    }
}
