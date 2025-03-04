using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;   
    [SerializeField] private int damage;
    [SerializeField] private string flavoring;
    [SerializeField] private GameObject damagePopup;
    private SpriteRenderer spriteRenderer;
    private GameObject popupStore;
    private Color ogColor;

    public global::System.Int32 Damage { get => damage; set => damage = value; }

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        ogColor = spriteRenderer.color;

        
    }
    void Update() {
        if (health <= 0) {
            
            Destroy(gameObject, 0.2f);
        }
    }

    IEnumerator Hurt(int damage, string flavor) {
        health -= damage;
        CreatePopup(damage, flavor);

        spriteRenderer.color = Color.red;

        float duration = 0.4f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(Color.red, ogColor, elapsedTime / duration);
            yield return null;
        }

        spriteRenderer.color = ogColor;

    }

    void CreatePopup(int damage, string flavor){
        GameObject newPopup = Instantiate(damagePopup, transform.position, Quaternion.identity);
        DamagePopup dp = newPopup.GetComponent<DamagePopup>();
        dp.DamageNumber = damage;
        dp.OutlineColor = GameManager.FlavorColorMap[flavor];
        dp.transform.SetParent(GameManager.PopupStore.transform);

    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "PlayerAttack") {
            GameObject item = collider.gameObject.transform.parent.gameObject;

            Melee melee = item.GetComponent<Melee>();

            StartCoroutine(Hurt(melee.AttackDamage, melee.Flavor));
        }
    }
    
}