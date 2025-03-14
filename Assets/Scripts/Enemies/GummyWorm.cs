using UnityEngine;

public class GummyWorm : MonoBehaviour  
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackRadius = 1f; // Example value for attack radius
    private Rigidbody2D body;
    private Animator animator;
    private float detectionRadius;
    //private bool isAttacking = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        detectionRadius = GetComponent<Enemy>().DetectionRadius;
    }

    void Update() {
        animator.SetFloat("horizontal", body.linearVelocity.x);
        animator.SetFloat("vertical", body.linearVelocity.y);

        if (body.linearVelocity.x < 0) {
            Vector2 rotator = new Vector3(transform.rotation.x, 180f);
            transform.rotation = Quaternion.Euler(rotator);
        } else if (body.linearVelocity.x > 0) {
            Vector2 rotator = new Vector3(transform.rotation.x, 0f);
            transform.rotation = Quaternion.Euler(rotator);
        }
    }

    void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        float distanceToPlayer = Vector2.Distance(body.position, SirGluten.playerPosition);
        bool detected = distanceToPlayer <= detectionRadius;    
        bool withinAttackRange = distanceToPlayer <= attackRadius;


        if (detected)
        {
            if (withinAttackRange)
            {
                // Play attack animation
        
                animator.SetTrigger("Attack");
                //isAttacking = true;
                Vector2 direction = (SirGluten.playerPosition - body.position).normalized;

                body.linearVelocity = direction * moveSpeed;
                animator.SetBool("isWalking", false);
            }
            else
            {
                // Move towards player and play walking animation
               
                Vector2 direction = (SirGluten.playerPosition - body.position).normalized;
                body.linearVelocity = direction * moveSpeed;
                animator.SetBool("isWalking", true);
                //isAttacking = false; // Ensure isAttacking is reset
            }
        }
        else
        {
            // Stop moving and play idle animation
    
            body.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
            //isAttacking = false;
        }    
    }
}