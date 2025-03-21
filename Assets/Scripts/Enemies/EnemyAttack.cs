using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField]private float timeTilCollision = 0.05f, timeTilDeath = 10f;

    private float elapsedTime = 0;
    private Collider hitbox;

    public global::System.Int32 Damage { get => damage; set => damage = value; }
    void Start(){
        hitbox = GetComponent<Collider>();

        Rigidbody2D spearBody= GetComponent<Rigidbody2D>();
        if (spearBody != null) {
            spearBody.linearVelocity = transform.up * speed; 
        }

        if (hitbox != null)
        {
            hitbox.enabled = false;
        }

        if (timeTilDeath >= 0) {
            Destroy(gameObject,timeTilDeath);
        }

    }

    void Update(){
        elapsedTime += Time.deltaTime;

        if (hitbox != null && elapsedTime >= timeTilCollision)
        {
            hitbox.enabled = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Walls" && timeTilDeath >= 0) {
            Destroy(gameObject);
        }
    }

    public void DestroyOnCollide() {
        if (timeTilDeath >= 0) Destroy(gameObject);
    }
}