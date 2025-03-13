using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField]private EnemyData enemyData;
    private int health;   
    private int damage;
    private string flavoring;
    [SerializeField] private GameObject damagePopup;
    [SerializeField] private GameObject deathParticle;
    private float xpMultiplier = 1f, goldMultiplier = 1f;
    [SerializeField] private List<AudioClip> hurtSFX;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Color ogColor;
    private bool isDying;
    private float detectionRadius = 10f;

    public global::System.Int32 Damage { get => damage; set => damage = value; }
    public global::System.Single DetectionRadius { get => detectionRadius; set => detectionRadius = value; }
    public EnemyData EnemyData { get => enemyData; set => enemyData = value; }

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        ogColor = spriteRenderer.color;

        health = enemyData.Health;
        damage = enemyData.Damage;
        flavoring = enemyData.Flavoring;
        xpMultiplier = enemyData.XpMultiplier;
        goldMultiplier = enemyData.GoldMultiplier;
        detectionRadius = enemyData.DetectionRadius;
    }
    void Update() {
        if (health <= 0 && !isDying) {
            GameManager.PlayParticle(deathParticle,transform.position);
            Death();
        }
    }

    void Death(){
        isDying = true;
        SirGluten.staticYeast += (int)Mathf.Round(Random.Range(1,25) * xpMultiplier);
        SirGluten.staticGold += (int)Mathf.Round(Random.Range(1,10) * goldMultiplier);

        enemyData.IsDiscovered = true;
        Destroy(gameObject, 0.2f);
        health--;
    }

    IEnumerator Hurt(int damage, string flavor) {
        int newDamage = damage;
        bool attackEffective = GameManager.IsEffective(flavor, flavoring);
        if (attackEffective) {
            newDamage = (int)Mathf.Ceil(newDamage * 1.5f);
        }
        
        health -= newDamage;
        CreatePopup(newDamage, flavor, attackEffective);

        if (hurtSFX.Count > 0) {
            audioSource.clip = hurtSFX[Random.Range(0,hurtSFX.Count)];
            audioSource.Play();
        }

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

    void CreatePopup(int damage, string flavor, bool attackEffective){
        GameObject newPopup = Instantiate(damagePopup, transform.position, Quaternion.identity);
        DamagePopup dp = newPopup.GetComponent<DamagePopup>();
        dp.DamageNumber = damage;
        dp.OutlineColor = GameManager.FlavorColorMap[flavor];
        dp.IsCritical = attackEffective;
        dp.transform.SetParent(GameManager.EffectStore.transform);

    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "PlayerAttack") {
            GameObject item = collider.gameObject.transform.parent.gameObject;

            Weapon weapon = item.GetComponent<Weapon>();

            StartCoroutine(Hurt(weapon.AttackDamage, weapon.Flavor));
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
       if (collision.gameObject.tag == "MagicProj") {


            MagicProj mp = collision.gameObject.GetComponent<MagicProj>();

            StartCoroutine(Hurt(mp.AttackDamage, mp.Flavor));
        } 
    }
    
}