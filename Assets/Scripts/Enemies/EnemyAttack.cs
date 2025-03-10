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

    public global::System.Int32 Damage { get => damage; set => damage = value; }
    void Start(){
        Rigidbody2D spearBody= GetComponent<Rigidbody2D>();
        if (spearBody != null) {
            spearBody.linearVelocity = transform.up * speed; 
        }
    }
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "Walls") {
            Destroy(gameObject);
        }
    }
}