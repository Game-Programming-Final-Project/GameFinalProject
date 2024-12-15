using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 3f; // Speed at which the enemy moves towards the player

    void Update()
    {
        if (player != null)
        {
            MoveTowardsPlayer();
            RotateTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate direction towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Ensure the enemy stays on the same vertical plane

        // Move the enemy towards the player
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void RotateTowardsPlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Keep rotation on the XZ plane

        // Calculate the rotation to look at the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
}
