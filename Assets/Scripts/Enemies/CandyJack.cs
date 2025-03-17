using System.Collections;
using UnityEngine;

public class CandyJack : MonoBehaviour
{
    [SerializeField] private float chargeSpeed = 5f;
    [SerializeField] private float attackCooldown = 5f; 
    [SerializeField] private float attackDelay = 0.5f;

    private Transform player;
    private Animator animator;
    private Rigidbody2D body;
    private bool isCharging = false;
    private Vector2 destPos;
    private EnemyData enemyData;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        
        enemyData = GetComponent<Enemy>().EnemyData;

        StartCoroutine(AttackSequence());
    }

    void Update()
    {
        animator.SetBool("isMoving", body.linearVelocity.magnitude > 0.5f);

        if (!isCharging) {
            destPos = player.position;
        } else {
            if (body.linearVelocity.x > 0.5f) {
                transform.localScale = new Vector3(1f, 1f, 1f);
            } else if (body.linearVelocity.x < -0.5f) {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
        Debug.Log(isCharging);
        
    }

    private IEnumerator AttackSequence()
    {
        while (Vector2.Distance(transform.position, player.position) > enemyData.DetectionRadius) 
            yield return null; 
        
        yield return new WaitForSeconds(attackDelay);

        isCharging = true;
        Vector2 direction = (destPos - (Vector2)transform.position).normalized;
        body.linearVelocity = direction * chargeSpeed; 

        while (body.linearVelocity.magnitude > 0.2f) yield return null;
        isCharging = false;

        yield return new WaitForSeconds(attackCooldown);

        animator.SetTrigger("isReady");

        StartCoroutine(AttackSequence());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerProj" && isCharging) {
            Vector2 direction = (destPos - (Vector2)transform.position).normalized;
            body.linearVelocity = direction * chargeSpeed; 

        } else if (collision.gameObject.tag == "Walls" || collision.gameObject.tag == "Player") {
            if (isCharging)
            {
                isCharging = false;
                body.linearVelocity = Vector2.zero;
            }
        }
    }
}