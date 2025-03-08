using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Marshmoblin : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] GameObject spear;
    [SerializeField] private float attackOffset = 0f;
    [SerializeField] private float cooldown = 2f;
    private EnemyData enemyData;
    [SerializeField] private List<AudioClip> attackSFX;
    
    private AudioSource audioSource;
    private Animator animator;
    
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        StartCoroutine(StartAttack());

        enemyData = GetComponent<Enemy>().EnemyData;
    }

    IEnumerator StartAttack(){
        yield return new WaitForSeconds(attackOffset);
        StartCoroutine(Attack());
    }

    void Update(){
        if (SirGluten.playerPosition.x > transform.position.x) {
            Vector2 rotator = new Vector3(transform.rotation.x, 180f);
            transform.rotation = Quaternion.Euler(rotator);
        } else {
            Vector2 rotator = new Vector3(transform.rotation.x, 0f);
            transform.rotation = Quaternion.Euler(rotator);
        }
    }

    IEnumerator Attack(){
        while (Vector2.Distance(transform.position, SirGluten.playerPosition) > enemyData.DetectionRadius) yield return null;

        yield return new WaitForSeconds(0.6f);
        animator.SetTrigger("attack");
        
        yield return new WaitForSeconds(cooldown);
        StartCoroutine(Attack());
    }

    void CreateSpear(){
        if (attackSFX.Count > 0) {
            audioSource.clip = attackSFX[Random.Range(0, attackSFX.Count)];
            audioSource.Play();
        }
        Vector2 spawnPosition = new Vector2(body.position.x + 0.6f, body.position.y + 0.3f);
        Vector2 directionToPlayer = (new Vector2(SirGluten.playerPosition.x, SirGluten.playerPosition.y) - (Vector2)transform.position).normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, directionToPlayer);
        GameObject newSpear = Instantiate(spear, spawnPosition, rotation);
        newSpear.transform.parent = GameManager.EffectStore.transform;
    }
}
