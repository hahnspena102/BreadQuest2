using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Foundue : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SirGluten sirGluten;
    private bool isFrozen;
    [SerializeField] private float speed = 6f;
    [SerializeField]private GameObject crystal;
    private Enemy enemy;
    //[SerializeField] GameObject projectile;
    //[SerializeField] AudioSource hauntSFX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {   
        if (crystal == null) {
            enemy.Health = 0;
        }
        if (isFrozen == true)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll; 
        }
        else
        {
             rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
        }

        if (animator != null && rb != null) {
            animator.SetBool("isMoving",!isFrozen);
        }

        if (sirGluten == null) return;

        Vector2 direction = ((Vector2)sirGluten.transform.position - rb.position).normalized;
        rb.linearVelocity = direction * speed;
/*
        if (sirGluten.Facing == "Left") {   
            isFrozen = rb.position.x < sirGluten.transform.position.x;
        } else if (sirGluten.Facing == "Right") {
            isFrozen = rb.position.x > sirGluten.transform.position.x;
        } else if (sirGluten.Facing == "Up") {
            isFrozen = rb.position.y > sirGluten.transform.position.y;
        } else {
            isFrozen = rb.position.y < sirGluten.transform.position.y;
        }
*/
        if (Mathf.Abs(rb.position.x - sirGluten.transform.position.x) > Mathf.Abs(rb.position.y - sirGluten.transform.position.y)) {
            if (rb.position.x < sirGluten.transform.position.x) {   
                isFrozen = sirGluten.Facing == "Left";
            } else if (rb.position.x > sirGluten.transform.position.x) {
                isFrozen = sirGluten.Facing == "Right";
            }
        } else {
            if (rb.position.y > sirGluten.transform.position.y) {   
                isFrozen = sirGluten.Facing == "Up";
            } else if (rb.position.x < sirGluten.transform.position.x) {
                isFrozen = sirGluten.Facing == "Down";
            }
        }

    }



    void Freeze(){

    }
}
