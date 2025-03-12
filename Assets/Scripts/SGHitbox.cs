using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class SGHitbox : MonoBehaviour
{
    private Rigidbody2D body;
    private SirGluten sirGluten;

    void Start() {
        sirGluten = transform.parent.gameObject.GetComponent<SirGluten>();
        body = transform.parent.gameObject.GetComponent<Rigidbody2D>();
    }
    
    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
//            Debug.Log("You take damage: " + enemy.Damage + "Current damage: " + health);
            Vector2 collisionPoint = collision.transform.position;
                Vector2 direction;

            if (collisionPoint.x > transform.position.x) {
                direction = new Vector2(-1f, 1f); 
            } else {
                direction = new Vector2(1f, 1f);
            }

            sirGluten.IsLocked = true;
            body.linearVelocity = direction * 2f;
            StartCoroutine(sirGluten.Hurt(enemy.Damage));
        } else if (collision.gameObject.tag == "EnemyAttack") {
            EnemyAttack enemyAttack = collision.gameObject.GetComponent<EnemyAttack>();
            StartCoroutine(sirGluten.Hurt(enemyAttack.Damage));
        }
    }
}
