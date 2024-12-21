using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;      // Pause menu paneli
    public GameObject controlsPanel; // Controls paneli
    public GameObject controlsMenuStart;
    private bool isPaused = false;   // Oyunun duraklatýlýp duraklatýlmadýðýný takip eder
    public GameObject startMenu;     // Start menu paneli

    private void Start()
    {
        startMenu.SetActive(true);  // Baþlangýç menüsünü göster
        pauseMenu.SetActive(false); // Pause menüsünü gizle
        Time.timeScale = 0f;        // Oyun duraklatýlýr (Baþlangýçta oyun duraklatýlmalý)
    }

    void Update()
    {
        // ESC tuþuna basýldýðýnda duraklatma menüsünü aç/kapat, ama baþlangýç menüsünde iken engelle
        if (Input.GetKeyDown(KeyCode.Escape) && !startMenu.activeSelf && !controlsMenuStart.activeSelf)
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
        pauseMenu.SetActive(false);  // Pause menüsü kapanýr
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
        pauseMenu.SetActive(true);      // Pause menüsünü aç
    }

    // Oyun baþlatma (Start Menu'den Play týklanýrsa)
    public void PlayGame()
    {
        startMenu.SetActive(false);  // Start menu'yi gizle
        Time.timeScale = 1f;         // Oyun baþlar
    }

    // Oyundan çýkma (Start Menu'den Exit týklanýrsa)
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;  // Oyunu kapat
    }
    public void BackToStartMenu()
    {
        controlsMenuStart.SetActive(false); // Kontroller panelini kapat
        startMenu.SetActive(true);      // Pause menüsünü aç
    }
    public void ShowControlsStart()
    {
        startMenu.SetActive(false);   // Pause menüsünü kapat
        controlsMenuStart.SetActive(true); // Kontroller panelini aç
    }
}
