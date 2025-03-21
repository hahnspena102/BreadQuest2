using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Frostling : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] GameObject projectile;
    [SerializeField] private float attackOffset = 0f;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private List<AudioClip> attackSFX;
    [SerializeField] private float range =  8f;
    private AudioSource audioSource;

    private float currentAngle = 0f;
    private float phase = 0f;
    private Color currentColor;
    
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        currentColor = GetComponent<SpriteRenderer>().color;
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(StartAttack());

    }

    void Update(){
        if (phase == 0) {
            body.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        } else {
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    IEnumerator StartAttack(){
        yield return new WaitForSeconds(attackOffset);
        StartCoroutine(Attack());
        StartCoroutine(PhaseLoop());
    }

    IEnumerator PhaseLoop(){
        yield return new WaitForSeconds(3f);
        phase = 1;
        
        yield return new WaitForSeconds(cooldown);
        while (Vector2.Distance(transform.position, SirGluten.playerPosition) > range) yield return null;
        if (attackSFX.Count > 0) {
            audioSource.clip = attackSFX[Random.Range(0, attackSFX.Count)];
            audioSource.Play();
        }
        
        phase = 0;
        StartCoroutine(PhaseLoop());
    }

    IEnumerator Attack(){
        currentAngle = (currentAngle + 30f) % 360;
        Quaternion newRotation = Quaternion.Euler(0, 0, currentAngle);

        while (phase == 1) yield return null;

        Vector2 spawnPosition = new Vector2(body.position.x, body.position.y);
        Vector2 directionToPlayer = (new Vector2(SirGluten.playerPosition.x, SirGluten.playerPosition.y) - (Vector2)transform.position).normalized;

        GameObject newProjectile = Instantiate(projectile, spawnPosition, newRotation);
        newProjectile.transform.parent = GameManager.EffectStore.transform;

        SpriteRenderer newProjSprite = newProjectile.GetComponent<SpriteRenderer>();
        newProjSprite.color = currentColor;

        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
        rb.angularVelocity = -360f * 2;

        yield return new WaitForSeconds(0.2f);
        StartCoroutine(Attack());
    }
}
