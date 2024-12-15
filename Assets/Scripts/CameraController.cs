using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset = new Vector3(0f, 10f, -10f); // Offset of the camera from the player
    public float smoothSpeed = 0.125f; // Speed for smooth damping

    void LateUpdate()
    {
        if (player != null)
        {
            // Desired position based on player position and offset
            Vector3 desiredPosition = player.position + offset;

            // Smoothly interpolate to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Keep the camera looking at the player
            transform.LookAt(player);
        }
    }
}
