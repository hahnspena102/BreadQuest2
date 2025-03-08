using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MagicProj : MonoBehaviour
{
    private int attackDamage;
    private string flavor;
    [SerializeField] private float speed;
    [SerializeField] private float duration = 5f;

    public global::System.Int32 AttackDamage { get => attackDamage; set => attackDamage = value; }
    public global::System.String Flavor { get => flavor; set => flavor = value; }

    void Start(){
        Rigidbody2D rb= GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.linearVelocity = rb.linearVelocity * speed; 
        }

        Destroy(gameObject, duration);
    }
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "Walls") {
            Destroy(gameObject);
        }
    }
}