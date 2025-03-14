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
    [SerializeField]private float timeTilCollision = 0.05f;

    private float elapsedTime;

    public global::System.Int32 Damage { get => damage; set => damage = value; }
    void Start(){
        Rigidbody2D spearBody= GetComponent<Rigidbody2D>();
        if (spearBody != null) {
            spearBody.linearVelocity = transform.up * speed; 
        }
        Destroy(gameObject,10f);
    }

    void Update(){
        elapsedTime += Time.deltaTime;
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Walls" && elapsedTime > timeTilCollision) {
            Destroy(gameObject);
        }
    }

    public void DestroyOnCollide() {
        Destroy(gameObject);
    }
}