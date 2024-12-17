using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    private Camera mainCamera;

    [Header("Stamina Settings")]
    public float maxStamina = 100f; // Maksimum stamina
    private float currentStamina;   // �u anki stamina
    public float staminaDrainRate = 10f; // Ko�arken saniyede azalan stamina miktar�
    public float staminaRegenRate = 5f;  // Ko�may� b�rakt���nda saniyede dolan stamina miktar�

    public Slider staminaBar;       // Stamina bar slider

    void Start()
    {
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Confined; // Keep the cursor within the game window
        currentStamina = maxStamina; // Ba�lang��ta maksimum stamina
        UpdateStaminaBar();          // Stamina bar�n� g�ncelle
    }

    void Update()
    {
        MovePlayer();
        RotateTowardsMouse();
        UpdateStamina();             // Stamina de�erlerini g�ncelle
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
            staminaBar.value = currentStamina / maxStamina; // Slider'� normalize et
        }
    }
}
