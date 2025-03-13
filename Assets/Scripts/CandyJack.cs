using System.Collections;
using UnityEngine;

public class CandyJack : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 5f; // Radius within which the player is detected
    [SerializeField] private float chargeSpeed = 5f; // Speed at which CandyJack charges towards the player
    [SerializeField] private float attackCooldown = 5f; // Cooldown period between attacks

    private Transform player;
    private Animator animator;
    private Rigidbody2D body;
    private bool isCharging = false;
    private bool isAttacking = false;
    private float attackTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (isCharging)
        {
            // Charge towards the player
            Vector2 direction = (player.position - transform.position).normalized;
            body.linearVelocity = direction * chargeSpeed;
        }
        else
        {
            body.linearVelocity = Vector2.zero;
        }

        if (attackTimer >= attackCooldown && !isAttacking && !isCharging)
        {
            // Detect the player within the detection radius
            if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
            {
                StartCoroutine(AttackSequence());
            }
            else
            {
                // Play idle animation
                animator.SetBool("isIdle", true);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isCharging", false);
            }
        }
    }

    private IEnumerator AttackSequence()
    {
        isAttacking = true;
        attackTimer = 0f;

        // Play attack animation
        animator.SetBool("isIdle", false);
        animator.SetBool("isAttacking", true);
        animator.SetBool("isCharging", false);

        // Wait for the attack animation to finish (assuming 1 second for the animation)
        yield return new WaitForSeconds(0.5f);

        // Wait for a delay before charging (e.g., 0.5 seconds)
        yield return new WaitForSeconds(0.5f);

        // Start charging towards the player
        isCharging = true;
        animator.SetBool("isIdle", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isCharging", true);

        // Wait for the charge duration (e.g., 1 second)
        yield return new WaitForSeconds(1f);

        // Stop charging and go back to idle
        isCharging = false;
        isAttacking = false;
        animator.SetBool("isIdle", true);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isCharging", false);
    }
}