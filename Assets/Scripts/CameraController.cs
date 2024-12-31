using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; 
    public Vector3 offset = new Vector3(0f, 10f, -10f); 
    public float smoothSpeed = 0.125f; 

    void LateUpdate()
    {
        if (player != null)
        {
            
            Vector3 desiredPosition = player.position + offset;

            
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            
            transform.LookAt(player);
        }
    }
}
