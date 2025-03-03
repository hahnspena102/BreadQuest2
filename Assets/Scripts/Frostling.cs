using UnityEngine;
using System.Collections;

public class Frostling : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] GameObject projectile;
    [SerializeField] private float attackOffset = 0f;
    [SerializeField] private float cooldown = 0.5f;

    private float currentAngle = 0f;
    private float phase = 0f;
    private Color currentColor;
    
    
    void Start()
    {
    
        body = GetComponent<Rigidbody2D>();
        currentColor = GetComponent<SpriteRenderer>().color;

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
        phase = 0;
        StartCoroutine(PhaseLoop());
    }

    IEnumerator Attack(){
        currentAngle = (currentAngle + 20f) % 360;
        Quaternion newRotation = Quaternion.Euler(0, 0, currentAngle);

        while (phase == 1) yield return null;

        Vector2 spawnPosition = new Vector2(body.position.x, body.position.y);
        Vector2 directionToPlayer = (new Vector2(SirGluten.playerPosition.x, SirGluten.playerPosition.y) - (Vector2)transform.position).normalized;

        GameObject newProjectile = Instantiate(projectile, spawnPosition, newRotation);
        newProjectile.transform.parent = transform;

        SpriteRenderer newProjSprite = newProjectile.GetComponent<SpriteRenderer>();
        newProjSprite.color = currentColor;

        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
        rb.angularVelocity = -360f * 2;

        yield return new WaitForSeconds(0.15f);
        StartCoroutine(Attack());
    }
}
