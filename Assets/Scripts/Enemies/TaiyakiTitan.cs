using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TaiyakiTitan : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SirGluten sirGluten;
    [SerializeField] GameObject projectile;
    [SerializeField] AudioSource bangSFX;
    [SerializeField]float shootCooldown = 5f;
    [SerializeField]Sprite projectileSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();

        StartCoroutine(TaiyakiAttack());
    }

    void Update()
    {
        if (animator != null && rb != null) {
            animator.SetBool("isMoving", rb.linearVelocity.magnitude > 0.2f);
        }
    }

    IEnumerator TaiyakiAttack(){
        while (rb.linearVelocity.magnitude > 0.2f) yield return null;
        yield return new WaitForSeconds(shootCooldown/2);
    
        animator.SetTrigger("shoot");

        yield return new WaitForSeconds(shootCooldown/2);
        StartCoroutine(TaiyakiAttack());
    }
    public void Shoot() 
    {
        Vector2 spawnPosition = new Vector2(rb.position.x + (transform.localScale.x * 0.7f), rb.position.y-0.2f);
        Vector2 directionToPlayer = (new Vector2(SirGluten.playerPosition.x, SirGluten.playerPosition.y) - (Vector2)transform.position).normalized;

        GameObject newProjectile = Instantiate(projectile, spawnPosition, Quaternion.identity);
        newProjectile.transform.SetParent(GameManager.ProjectileStore.transform);

        newProjectile.GetComponent<Enemy>().Flavoring = GetComponent<Enemy>().Flavoring;
        newProjectile.GetComponent<SpriteRenderer>().sprite = projectileSprite;
    }
}
