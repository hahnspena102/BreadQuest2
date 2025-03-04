using UnityEngine;

public class SimpleMovement : MonoBehaviour  
{
    [SerializeField] private float detectionRadius = 20f;
    [SerializeField] private float stopRadius = 0f;
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D body;
    private Animator animator;

    //private bool isMoving;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        bool detected = Vector2.Distance(body.position, SirGluten.playerPosition) <= detectionRadius;    
        bool stop = Vector2.Distance(body.position, SirGluten.playerPosition) >= stopRadius;
        if (detected && stop)
        {
            Vector2 direction = (SirGluten.playerPosition - body.position).normalized;
            body.linearVelocity = direction * moveSpeed;
            //isMoving = true;
        }
        else
        {
            body.linearVelocity = Vector2.zero;
           // isMoving = false;
        }    
    }

    /*
    void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    */
}
