using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
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

        if(movement.magnitude > 0)
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
