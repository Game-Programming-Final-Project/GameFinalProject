using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Confined; // Keep the cursor within the game window
    }

    void Update()
    {
        MovePlayer();
        RotateTowardsMouse();
    }

    void MovePlayer()
    {
        // Input for movement
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float vertical = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        // Determine speed based on Shift key
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Apply movement directly to the transform
        transform.position += movement * speed * Time.deltaTime;
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
}
