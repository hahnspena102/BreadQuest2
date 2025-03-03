using UnityEngine;

public class SimpleMovement : MonoBehaviour  
{
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private float moveSpeed = 30f;
    private Rigidbody2D body;
    private Animator animator;

    private bool isMoving;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        animator.SetFloat("horizontal", body.linearVelocity.x);
        animator.SetFloat("vertical", body.linearVelocity.y);
    }

    void FixedUpdate()
    {
        MoveTowardsPlayer();
    }


    void MoveTowardsPlayer()
    {
        bool detected = Vector2.Distance(body.position, SirGluten.playerPosition) <= detectionRadius;    
        if (detected)
        {
            Vector2 direction = (SirGluten.playerPosition - body.position).normalized;
            body.linearVelocity = direction * moveSpeed;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }    
    }

    /*
    void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    */
}
