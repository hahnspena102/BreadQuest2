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
    
    void OnTriggerStay2D(Collider2D collider) {
        if (collider.gameObject.tag == "Enemy") {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
//            Debug.Log("You take damage: " + enemy.Damage + "Current damage: " + health);
            Vector2 colliderPoint = collider.transform.position;
                Vector2 direction;

            if (colliderPoint.x > transform.position.x) {
                direction = new Vector2(-1f, 1f); 
            } else {
                direction = new Vector2(1f, 1f);
            }

            

            sirGluten.IsLocked = true;
            body.linearVelocity = direction * 2f;
            StartCoroutine(sirGluten.Hurt(enemy.Damage, false));

            if (enemy.DeathOnCollide) enemy.Health = 0;
        } else if (collider.gameObject.tag == "EnemyAttack") {
            EnemyAttack enemyAttack = collider.gameObject.GetComponent<EnemyAttack>();
            StartCoroutine(sirGluten.Hurt(enemyAttack.Damage, false));
            enemyAttack.DestroyOnCollide();
        } else if (collider.gameObject.tag == "Spikes"){
            Spikes spikes = collider.gameObject.GetComponent<Spikes>();
            if(spikes.IsActive){
                StartCoroutine(sirGluten.Hurt(10, false));
            }
        } else if (collider.gameObject.tag == "BossAttack") {
            BossAttack bossAttack = collider.gameObject.GetComponent<BossAttack>();
            sirGluten.IsLocked = true;
            Rigidbody2D attackRB = collider.gameObject.GetComponent<Rigidbody2D>();
            body.linearVelocity = attackRB.linearVelocity * 1f;
            StartCoroutine(sirGluten.Hurt(bossAttack.Damage,true));    
        }         
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "BossAttack") {
            sirGluten.IsLocked = false;
        } 
    }
}


