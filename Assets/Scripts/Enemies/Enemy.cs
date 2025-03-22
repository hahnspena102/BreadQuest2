using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField]private EnemyData enemyData;
    private int health, maxHealth;   
    private int damage;
    private string flavoring;
    [SerializeField]private int bonusHealth;
    [SerializeField] private GameObject damagePopup;
    [SerializeField] private GameObject deathParticle;
    private float xpMultiplier = 1f, goldMultiplier = 1f;
    [SerializeField] private List<AudioClip> hurtSFX;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Color ogColor;
    private bool isDying;
    [SerializeField]private float detectionRadius = 10f;
    private float defense;
    [SerializeField]private bool deathOnCollide;
    private float invincibility;
    private SirGluten sirGluten;

    public global::System.Int32 Damage { get => damage; set => damage = value; }
    public global::System.Single DetectionRadius { get => detectionRadius; set => detectionRadius = value; }
    public EnemyData EnemyData { get => enemyData; set => enemyData = value; }
    public global::System.Int32 Health { get => health; set => health = value; }
    public global::System.Int32 MaxHealth { get => maxHealth; set => maxHealth = value; }
    public global::System.Boolean DeathOnCollide { get => deathOnCollide; set => deathOnCollide = value; }
    public global::System.String Flavoring { get => flavoring; set => flavoring = value; }

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        ogColor = spriteRenderer.color;

        maxHealth = enemyData.Health + bonusHealth;
        health = maxHealth;
        damage = enemyData.Damage;
        flavoring = enemyData.Flavoring;
        xpMultiplier = enemyData.XpMultiplier;
        goldMultiplier = enemyData.GoldMultiplier;
        detectionRadius = enemyData.DetectionRadius;
        defense = enemyData.Defense;

        GameObject glut = GameObject.Find("SirGluten");
        sirGluten = glut.GetComponent<SirGluten>();
    }
    void Update() {
        if (health <= 0 && !isDying) {
            GameManager.PlayParticle(deathParticle,transform.position);
            isDying = true;
            Destroy(gameObject, 0f);
        }
        invincibility -= Time.deltaTime;
        invincibility = Mathf.Clamp(invincibility, 0f ,1f);
    }
/*
    void Death(){
        isDying = true;
        SirGluten.staticYeast += (int)Mathf.Round(Random.Range(1,25) * xpMultiplier);
        SirGluten.staticGold += (int)Mathf.Round(Random.Range(1,10) * goldMultiplier);

        enemyData.IsDiscovered = true;
        Destroy(gameObject, 0.4f);
        health--;
    }
    */

    void OnDestroy(){
        isDying = true;
        GameManager.PlayParticle(deathParticle,transform.position);
        SirGluten.staticYeast += (int)Mathf.Round(Random.Range(1,25) * xpMultiplier);
        SirGluten.staticGold += (int)Mathf.Round(Random.Range(1,10) * goldMultiplier);

        enemyData.IsDiscovered = true;
        health--;  
    }

    IEnumerator Hurt(int damage, string flavor) {
        invincibility += 0.2f;

        int newDamage = damage;
        bool attackEffective = GameManager.IsEffective(flavor, flavoring);
        if (attackEffective) {
            if (sirGluten.PassiveSlot != null) {
                newDamage = (int)Mathf.Ceil(newDamage * (1.5f + sirGluten.PassiveSlot.CriticalBonus));
            } else {
                newDamage = (int)Mathf.Ceil(newDamage * 1.5f);
            }

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
        if (collider.gameObject.tag == "PlayerAttack" && invincibility == 0) {
            GameObject item = collider.gameObject.transform.parent.gameObject;

            Weapon weapon = item.GetComponent<Weapon>();
            int totalDamage = (int)Mathf.Round(weapon.AttackDamage - (weapon.AttackDamage * defense));
            if (totalDamage < 0) totalDamage = 0;
            StartCoroutine(Hurt(totalDamage, weapon.Flavor));
        }
        if (collider.gameObject.tag == "PlayerProj") {


            PlayerProj mp = collider.gameObject.GetComponent<PlayerProj>();
            int totalDamage = (int)Mathf.Round(mp.AttackDamage - (mp.AttackDamage * defense));
            if (totalDamage < 0) totalDamage = 0;
            StartCoroutine(Hurt(totalDamage, mp.Flavor));
        } 
    }

    void OnCollisionEnter2D(Collision2D collision) {
       if (collision.gameObject.tag == "PlayerProj") {
            PlayerProj mp = collision.gameObject.GetComponent<PlayerProj>();
            int totalDamage = (int)Mathf.Round(mp.AttackDamage - (mp.AttackDamage * defense));
            if (totalDamage < 0) totalDamage = 0;
            StartCoroutine(Hurt(totalDamage, mp.Flavor));
        }
    }
    
}