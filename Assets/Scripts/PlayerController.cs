using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    private Camera mainCamera;
    private Health health;
    private FinanceManager financeManager;
    [Header("Stamina Settings")]
    public float maxStamina = 100f; // Maksimum stamina
    public float currentStamina;   // Þu anki stamina
    public float staminaDrainRate = 10f; // Koþarken saniyede azalan stamina miktarý
    public float staminaRegenRate = 5f;  // Koþmayý býraktýðýnda saniyede dolan stamina miktarý
    public TextMeshProUGUI healthcounter;
    public Slider staminaBar;       // Stamina bar slider

    void Start()
    {
        health = GetComponent<Health>();
        financeManager = FindObjectOfType<FinanceManager>();
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Confined; // Keep the cursor within the game window
        currentStamina = maxStamina; // Baþlangýçta maksimum stamina
        UpdateStaminaBar();          // Stamina barýný güncelle
        
    }

    void Update()
    {
        healthcounter.text = health.getCurrentHealth() + "/" + health.getMaxHealth();
        MovePlayer();
        RotateTowardsMouse();
        UpdateStamina();             // Stamina deðerlerini güncelle
    }

    void MovePlayer()
    {
        // Input for movement
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float vertical = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        // Determine speed based on Shift key and stamina availability
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0.1;
        float speed = isRunning ? runSpeed : walkSpeed;

        // Apply movement directly to the transform
        transform.position += movement * speed * Time.deltaTime;

        // Drain stamina if running
        if (isRunning)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // Sýnýrlandýr
        }
    }

    void RotateTowardsMouse()
    {
        // Plane for mouse raycast
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (playerPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = (targetPoint - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void UpdateStamina()
    {
        // Regenerate stamina when not running
        if (!Input.GetKey(KeyCode.LeftShift) || currentStamina <= 0)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // Sýnýrlandýr
        }

        UpdateStaminaBar(); // Slider'ý güncelle
    }

    void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina; // Slider'ý normalize et
        }
    }
    public void BuyIncreaseMaxStamina(int cost)
    {
        if (financeManager.SpendSoul(cost))
        {
            maxStamina += 10;
            staminaBar.maxValue = maxStamina;
            UpdateStaminaBar();
        }
    }
    public void BuyStaminaRegenRate(int cost)
    {
        if (financeManager != null && financeManager.SpendSoul(cost))
        {
            staminaRegenRate += ((staminaRegenRate * 3) / 10);

            Debug.Log("Firerate increased by %30!");
        }
        else
        {
            Debug.Log("Not enough souls to increase Max Health!");
        }
    }

}
