using UnityEngine;
using System.Collections;

public class DangOh : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] GameObject projectile;
    [SerializeField] private float attackOffset = 0f;
    [SerializeField] private float cooldown = 2f;
    
    
    void Start()
    {
    
        body = GetComponent<Rigidbody2D>();

        StartCoroutine(StartAttack());

    }

    IEnumerator StartAttack(){
        yield return new WaitForSeconds(attackOffset);
        StartCoroutine(Attack());
    }

    IEnumerator Attack(){
        /*while (Vector2.Distance(SirGluten.playerPosition.position, player.position) > 16f) {
            yield return null;
        } 
        */
        
        yield return new WaitForSeconds(0.6f);
        Vector2 spawnPosition = new Vector2(body.position.x, body.position.y);
        Vector2 directionToPlayer = (new Vector2(SirGluten.playerPosition.x, SirGluten.playerPosition.y) - (Vector2)transform.position).normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, directionToPlayer);
        GameObject newProjectile = Instantiate(projectile, spawnPosition, rotation);
        newProjectile.transform.parent = transform;

        SpriteRenderer newProjSprite = newProjectile.GetComponent<SpriteRenderer>();
        newProjSprite.color = new Color(0.1f,0.1f,0.1f);

        
        yield return new WaitForSeconds(cooldown);
        StartCoroutine(Attack());
    }
}
