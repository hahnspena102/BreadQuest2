using UnityEngine;

// trying to implement slime movement that chases a player within a specific radius
// this script is attached to the slime prefab


public class FruitSlime : MonoBehaviour  
{
    public float detectionRadius = 5f; // Detection range for the player
    public float moveSpeed = 4f; // Speed of movement
    public LayerMask playerLayer; // Assign "Player" layer in Inspector

    private Transform player;
    private Rigidbody2D rb;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D component
    }

    void Update()
    {
        DetectPlayer();
    }

    void FixedUpdate()
    {
        if (isChasing && player != null)
        {
            MoveTowardsPlayer();
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Stop moving when not chasing
        }
    }

    void DetectPlayer() 
    {
        // Check if player is within detection radius
        Collider2D detected = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (detected != null)
        {
            player = detected.transform; // Assign player reference
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized; // Get direction to player
        rb.linearVelocity = direction * moveSpeed; // Move in that direction
    }

    // Draw the detection radius in Scene view for debugging
    void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
