using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Animator animator;
    public Transform player; 
    public float moveSpeed = 3f;
    public float damage = 10f;  
    private bool isPlayerInRange = false; 
    private float damageCooldown = 1f; 
    private float lastDamageTime = 0f; 
    public float stopDistance = 1.2f;
    private bool canFollowPlayer = true;
    public float bossSpawnDelay = 2f;

    void Start()
    {
        if (gameObject.CompareTag("Boss")) 
        {
            canFollowPlayer = false; 
            Invoke(nameof(EnableFollowForBoss), bossSpawnDelay); 
        }

        if (player == null) 
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform; 
            }
            else
            {
                Debug.LogWarning("Player object not found in the scene. Make sure the Player has the correct tag.");
            }
        }
        
    }

    void Update()
    {
        if (player != null && canFollowPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > stopDistance) 
            {
                MoveTowardsPlayer();
                RotateTowardsPlayer();
            }
            else
                animator.SetTrigger("EnemyAttackTrigger");

            
            if (isPlayerInRange && Time.time - lastDamageTime >= damageCooldown)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage); 
                    lastDamageTime = Time.time; 
                }
            }
        }
    }

    void MoveTowardsPlayer()
    {
        
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; 

        
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void RotateTowardsPlayer()
    {
       
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; 

        
        Quaternion targetRotation = Quaternion.LookRotation(direction);

       
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            isPlayerInRange = false; 
        }
    }
    private void EnableFollowForBoss()
    {
        canFollowPlayer = true;
        Debug.Log("Boss oyuncuyu takip etmeye baþladý.");
    }
}
