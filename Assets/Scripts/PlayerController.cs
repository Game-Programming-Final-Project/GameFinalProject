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
    public float maxStamina = 100f; 
    public float currentStamina;   
    public float staminaDrainRate = 10f; 
    public float staminaRegenRate = 5f;  
    public TextMeshProUGUI healthcounter;
    public Slider staminaBar;       
    private bool isDead;
    
    void Start()
    {
        
        health = GetComponent<Health>();
        financeManager = FindObjectOfType<FinanceManager>();
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Confined; 
        currentStamina = maxStamina; 
        UpdateStaminaBar();         
        
    }

    void Update()
    {
        if (isDead) return;
        healthcounter.text = health.getCurrentHealth() + "/" + health.getMaxHealth();
        MovePlayer();
        RotateTowardsMouse();
        UpdateStamina();            
    }

    void MovePlayer()
    {
        
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");  

       
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

       
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0.1;
        float speed = isRunning ? runSpeed : walkSpeed;

        
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

        
        if (isRunning)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); 
        }
    }

    void RotateTowardsMouse()
    {
        
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
        
        if (!Input.GetKey(KeyCode.LeftShift) || currentStamina <= 0)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        UpdateStaminaBar(); 
    }

    void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina; 
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
    public void UpdateHealtCounter()
    {
        healthcounter.text = health.getCurrentHealth() + "/" + health.getMaxHealth();
    }

}
