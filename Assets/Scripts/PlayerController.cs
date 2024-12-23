using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public Animator animator;
    private Camera mainCamera;
    private Health health;
    private FinanceManager financeManager;
    [Header("Stamina Settings")]
    public float maxStamina = 100f; // Maksimum stamina
    public float currentStamina;   // �u anki stamina
    public float staminaDrainRate = 10f; // Ko�arken saniyede azalan stamina miktar�
    public float staminaRegenRate = 5f;  // Ko�may� b�rakt���nda saniyede dolan stamina miktar�
    public TextMeshProUGUI healthcounter;
    public Slider staminaBar;       // Stamina bar slider
    private bool isDead;

    void Start()
    {
        health = GetComponent<Health>();
        financeManager = FindObjectOfType<FinanceManager>();
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Confined; // Keep the cursor within the game window
        currentStamina = maxStamina; // Ba�lang��ta maksimum stamina
        UpdateStaminaBar();          // Stamina bar�n� g�ncelle
        
    }

    void Update()
    {
        if (isDead) return;
        healthcounter.text = health.getCurrentHealth() + "/" + health.getMaxHealth();
        MovePlayer();
        RotateTowardsMouse();
        UpdateStamina();             // Stamina de�erlerini g�ncelle
        //if (Input.GetMouseButtonDown(0))
        //    animator.SetTrigger("ShootTrigger");
    }

    void MovePlayer()
    {
           
        // Input for movement
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float vertical = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow
        
        // Calculate movement direction
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        
        // Determine speed based on Shift key
        float speed = isRunning ? runSpeed : walkSpeed;

        // Apply movement directly to the transform
        transform.position += movement * speed * Time.deltaTime;

        if (movement.magnitude > 0)
        {
            if (isRunning)
            {
                animator.ResetTrigger("RunTrigger");
                animator.SetTrigger("RunFastTrigger");
            }
            else
            {
                animator.ResetTrigger("RunFastTrigger");
                animator.SetTrigger("RunTrigger");
            }
        }

        else
        {
            animator.ResetTrigger("RunTrigger");
            animator.ResetTrigger("RunFastTrigger");
            animator.SetTrigger("IdleTrigger");
        }

        // Drain stamina if running
        if (isRunning)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // S�n�rland�r
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
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // S�n�rland�r
        }

        UpdateStaminaBar(); // Slider'� g�ncelle
    }

    void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina; // Slider'� normalize et
            staminaBar.maxValue = maxStamina;
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
            staminaRegenRate += ((staminaRegenRate * 2) / 10);

            Debug.Log("Firerate increased by %20!");
        }
        else
        {
            Debug.Log("Not enough souls to increase Firerate!");
        }
    }
    public void IsPlayerDead()
    {
        isDead = true;

    }

}
