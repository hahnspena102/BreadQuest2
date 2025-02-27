using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;   
    [SerializeField] private int damage;

    public global::System.Int32 Damage { get => damage; set => damage = value; }

    void Update() {
        if (health == 0) {
            
            Destroy(gameObject, 0.2f);
        }
    }
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "PlayerAttack") {
            GameObject item = collider.gameObject.transform.parent.gameObject;

            Melee melee = item.GetComponent<Melee>();
            
            Debug.Log(melee.AttackDamage);

            health-= melee.AttackDamage;
        }
    }
}