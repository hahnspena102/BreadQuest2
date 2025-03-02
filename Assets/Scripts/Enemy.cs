using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;   
    [SerializeField] private int damage;
    [SerializeField] private string flavoring;
    private SpriteRenderer spriteRenderer;

    public global::System.Int32 Damage { get => damage; set => damage = value; }

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update() {
        if (health == 0) {
            
            Destroy(gameObject, 0.2f);
        }
    }

    IEnumerator Hurt(int damage) {
        health -= damage;

        Color ogColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;

        float duration = 0.4f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(Color.red, Color.white, elapsedTime / duration);
            yield return null;
        }

        spriteRenderer.color = ogColor;

    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "PlayerAttack") {
            GameObject item = collider.gameObject.transform.parent.gameObject;

            Melee melee = item.GetComponent<Melee>();

            StartCoroutine(Hurt(melee.AttackDamage));
        }
    }
}