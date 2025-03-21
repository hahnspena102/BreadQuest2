using UnityEngine;

public class Grahoblin : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SirGluten sirGluten;
    [SerializeField] GameObject attackHitbox;
    [SerializeField] AudioSource swingSFX;
    private AStarPathfinding astar;

    private float elapsedTime, attackCooldown = 2f;
    private int isAttacking;

    public global::System.Int32 IsAttacking { get => isAttacking; set => isAttacking = value; }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
        astar = GetComponent<AStarPathfinding>();
    }

    void Update()
    {
        if (isAttacking == 1)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll; 
        }
        else
        {
             rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
        }
        
        if (animator != null && rb != null)
        {
            if (animator != null && rb != null) {
                animator.SetBool("isMoving",rb.linearVelocity.magnitude > 0f);
            }

            Vector2 directionToSirGluten = (Vector2)sirGluten.transform.position - (Vector2)transform.position;

            if (Vector2.Distance((Vector2)sirGluten.transform.position, rb.position) < 2f)
            {
                elapsedTime += Time.deltaTime;
            }
        }
        
        if (elapsedTime > attackCooldown) {
            animator.SetTrigger("attack");
            elapsedTime = 0;
        }
    }


    public void ToggleSwing(int i)
    {   
        if (i == 1) {
            attackHitbox.SetActive(true);
            if (swingSFX != null) {
                swingSFX.pitch = Random.Range(1.00f - 0.10f, 1.00f + 0.10f);
                swingSFX.Play();
            }
        } else {
            attackHitbox.SetActive(false);
        }
    }
}
