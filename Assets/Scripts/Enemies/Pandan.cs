using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Pandan : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SirGluten sirGluten;
    [SerializeField] GameObject projectile;
    [SerializeField] AudioSource bangSFX;
    [SerializeField] LayerMask sirGlutenLayer;
    [SerializeField]float shootCooldown = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();

        StartCoroutine(PandanAttack());
    }

    void Update()
    {
        if (animator != null && rb != null) {
            animator.SetBool("isMoving", rb.linearVelocity.magnitude > 0.2f);
        }
    }

    IEnumerator PandanAttack(){
        while (rb.linearVelocity.magnitude > 0.2f || !CheckLOS()) yield return null;
        yield return new WaitForSeconds(shootCooldown/2);
    
        animator.SetTrigger("shoot");

        yield return new WaitForSeconds(shootCooldown/2);
        StartCoroutine(PandanAttack());
    }

    private bool CheckLOS()
    {
        Vector2 directionToSirGluten = (sirGluten.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToSirGluten, Mathf.Infinity, sirGlutenLayer);

        return (hit.collider != null && hit.collider.gameObject == sirGluten.gameObject);

    }

    public void ShootGun() 
    {

        Vector2 spawnPosition = new Vector2(rb.position.x, rb.position.y);
        Vector2 directionToPlayer = (new Vector2(SirGluten.playerPosition.x, SirGluten.playerPosition.y) - (Vector2)transform.position).normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, directionToPlayer);

        GameObject newProjectile = Instantiate(projectile, spawnPosition, rotation);
        newProjectile.transform.parent = GameManager.EffectStore.transform;

        //Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
        //rb.angularVelocity = -360f * 2;
        //bangSFX.pitch = Random.Range(1.00f - 0.10f, 1.00f + 0.10f);
        //bangSFX.Play();
    }
}
