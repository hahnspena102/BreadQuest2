using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class SirGluten : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float verticalInput, horizontalInput;
    private float speed = 4f;
    

    // STATS
    [SerializeField]private SaveData curSaveData;
    private int health, glucose, yeast, yeastLevel;
    private int maxHealth=100, maxGlucose=30, maxYeast, maxYeastLevel = 10;

    [SerializeField] private GameObject damagePopup;
    

    // BOOLS
    private bool isAttacking, isHurting, isLocked, isAnimationLocked, isNavigatingUI;

    // INVENTORY
    private GameObject hoveredWeapon;
    private Weapon hoveredWeaponItem;
    private Weapon mainSlot,subSlot;
    [SerializeField]private Passive passiveSlot;
    private int healthPotions, glucosePotions;
    private int hPotTimer, hPotCooldown = 30, gPotTimer, gPotCooldown = 30;
    private int gold;

    // STATICS
    public static Vector2 playerPosition;

    private int weaponAnimationFrame = 0;
    public static int staticYeast = 0, staticGold = 0;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        
        InitStats();
        UpdateStats();
        StartCoroutine(OneSecondCoroutine());
        
        StartCoroutine(LoadSaveData());
        
    }

    IEnumerator LoadSaveData(){
        yield return new WaitForSeconds(0.01f);
        yeast = curSaveData.Yeast;
        yeastLevel = curSaveData.YeastLevel;
        gold = curSaveData.Gold;
        
        passiveSlot = curSaveData.Passive;
        healthPotions = curSaveData.HealthPotions;
        glucosePotions = curSaveData.GlucosePotions;

        GameObject foundMainWeapon = GameManager.FindWeapon(curSaveData.MainSlotId);
        if (foundMainWeapon != null) {
            GameObject newMainSlot = Instantiate(foundMainWeapon, transform.position, Quaternion.identity);
            newMainSlot.transform.SetParent(transform);
            mainSlot = newMainSlot.GetComponent<Weapon>();
            yield return new WaitForSeconds(0.01f);
            newMainSlot.transform.GetChild(0).gameObject.SetActive(false);
        }
        GameObject foundSubWeapon = GameManager.FindWeapon(curSaveData.SubSlotId);
        if (foundSubWeapon != null) {
            GameObject newSubSlot = Instantiate(foundSubWeapon, transform.position, Quaternion.identity);
            newSubSlot.transform.SetParent(transform);
            subSlot = newSubSlot.GetComponent<Weapon>();
            yield return new WaitForSeconds(0.01f);
            newSubSlot.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void SaveData() {
        curSaveData.Yeast = yeast;
        curSaveData.YeastLevel = yeastLevel;
        curSaveData.Gold = gold;
        if (mainSlot != null) {
            curSaveData.MainSlotId = mainSlot.GetComponent<Weapon>().Id;
        } else {
            curSaveData.MainSlotId = "";
        }
        if (subSlot != null) {
            curSaveData.SubSlotId = subSlot.GetComponent<Weapon>().Id;
        } else {
            curSaveData.SubSlotId = "";
        }
        
        curSaveData.Passive = passiveSlot;
        curSaveData.HealthPotions = healthPotions;
        curSaveData.GlucosePotions = glucosePotions;
    }

    // Initializes the Stat Values and Slider/Text variables.
    void InitStats(){
        health = maxHealth;
        glucose = maxGlucose;
        //yeast = 0;
       // yeastLevel = 1;
    }

    private IEnumerator OneSecondCoroutine()
    {
        while (true) {
            yield return new WaitForSeconds(1f);
            if (passiveSlot != null) {
                if (health < maxHealth && passiveSlot != null) health += passiveSlot.HealthRegeneration;
                if (glucose < maxGlucose && passiveSlot != null) glucose += 1 + passiveSlot.GlucoseRegeneration;
            } else {
                if (glucose < maxGlucose) glucose += 1;
            }
            if (hPotTimer > 0) hPotTimer--;
            if (gPotTimer > 0) gPotTimer--;
        }
    }
    
    void UpdateStats(){
        maxYeast = (int)Mathf.Round((Mathf.Pow(1.3f,yeastLevel)) * 100);
        //Debug.Log(maxYeast);
        yeast = Mathf.Max(yeast, staticYeast);
        if (yeast >= maxYeast && yeastLevel < maxYeastLevel) {
            yeast = 0;
            staticYeast = 0;
            yeastLevel += 1;
            maxHealth = 100 + (yeastLevel * 20);
            maxGlucose = 30 + (yeastLevel * 2);
            health = maxHealth;
            glucose = maxGlucose;
        }        
        if (yeastLevel >= maxYeastLevel) {
            yeast = maxYeast;
        }

        if (staticGold > 0) {
            gold = gold + staticGold;
            staticGold = 0;
        }

        health = Mathf.Clamp(health, 0 ,maxHealth);
        glucose = Mathf.Clamp(glucose, 0 ,maxGlucose);
    }
    
    void Update() {
        // Statics
        playerPosition = body.position;

        // Stats
        UpdateStats();

        if (health <= 0) { 
            SceneManager.LoadScene("GameOver");
        }

        // Movement
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");

        //if (Input.GetKeyDown(KeyCode.P)) yeast += 50;

        

        if (Input.GetKeyDown(KeyCode.E) && hoveredWeaponItem != null && !isAttacking) {
            Equip();
        }
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking) {
            Swap();
        }
        if (Input.GetKeyDown(KeyCode.Q) && mainSlot != null && !isAttacking) {
            Drop();
        }
        if ((Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.C)) && !isAttacking) {
            UseHealthPotion();
        }
        if ((Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.X)) && !isAttacking) {
            UseGlucosePotion();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SaveData();
            SceneManager.LoadScene("MainMenu");
        }
        


        if (!isLocked) MovePlayer();    

        // ANimations
        if (!isAnimationLocked) {
            animator.SetFloat("horizontal", Mathf.Abs(body.linearVelocity.x));
            animator.SetFloat("vertical", body.linearVelocity.y);
            animator.SetBool("isMoving", body.linearVelocity.x != 0 || body.linearVelocity.y != 0);
            animator.SetBool("isHurting", isHurting);
        }
        if (!isHurting) animator.SetBool("isHurting", isHurting);
    }

    void MovePlayer(){
        float newSpeed = speed;
        if (passiveSlot != null) newSpeed += passiveSlot.MovementBonus;
        body.linearVelocity = new Vector2(horizontalInput,verticalInput).normalized * newSpeed;
        
        if (!isAttacking) {
            if (horizontalInput < 0) {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            } else if (horizontalInput > 0) {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            if (verticalInput != 0 && horizontalInput == 0) {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }

    void Equip() {
        if (mainSlot != null && subSlot != null) Drop();
        if (mainSlot != null && subSlot == null) subSlot = mainSlot;

        mainSlot = hoveredWeaponItem;

        GameObject hoveredWeaponDropped = hoveredWeapon.transform.GetChild(0).gameObject;
        
    
        hoveredWeaponDropped.SetActive(false);
        hoveredWeapon.transform.SetParent(transform);   

        mainSlot.transform.position = body.position;
        mainSlot.transform.localRotation = Quaternion.identity;
    }

    void Swap() {
        if (subSlot == null) return;

        Weapon tempSlot = subSlot;
        subSlot = mainSlot;
        mainSlot = tempSlot;
    }

    void Drop() {
        if (mainSlot == null) return;

        GameObject items = GameObject.Find("ItemStore");
        GameObject itemDropped = hoveredWeapon.transform.GetChild(0).gameObject;

        if (items != null && itemDropped != null) {
            RectTransform mainSlotRect = mainSlot.GetComponent<RectTransform>();

            if (mainSlotRect != null) {
                mainSlot.transform.SetParent(items.transform);
                mainSlotRect.rotation = Quaternion.identity;
                mainSlotRect.anchoredPosition = body.position;
            }
            itemDropped = mainSlot.transform.GetChild(0).gameObject;
            itemDropped.transform.position = transform.position;
            
            itemDropped.gameObject.SetActive(true);

            mainSlot = null;
        } else {
            return;
        }

        if (subSlot != null) {
            mainSlot = subSlot;
            subSlot = null;
            
        }
    }

    void UseHealthPotion() {
        if (healthPotions <= 0 || hPotTimer > 0 || health >= maxHealth) return;
        healthPotions--;

        health += 50;
        health = Mathf.Clamp(health, 0, maxHealth);
        hPotTimer += hPotCooldown;
    }
    
    void UseGlucosePotion() {
        if (glucosePotions <= 0 || gPotTimer > 0 || glucose >= maxGlucose) return;
        glucosePotions--;

        glucose += 30;
        glucose = Mathf.Clamp(glucose, 0, maxGlucose);
        gPotTimer += gPotCooldown;
    }
    public IEnumerator Hurt(int damage){
        if (isHurting) yield break;
        isHurting = true;

        int totalDamage = damage;
        if (passiveSlot != null) totalDamage -= passiveSlot.DefensiveBonus;
        if (totalDamage < 0) totalDamage = 0;
        health -= totalDamage;
        CreatePopup(totalDamage);

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
        isHurting = false;
        isLocked = false;
    }

    void CreatePopup(int damage){
        GameObject newPopup = Instantiate(damagePopup, transform.position, Quaternion.identity);
        DamagePopup dp = newPopup.GetComponent<DamagePopup>();
        dp.DamageNumber = damage;
        dp.OutlineColor = Color.red;
        dp.IsPlayerHurt = true;
        dp.transform.SetParent(GameManager.EffectStore.transform);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Item") {
            if (hoveredWeaponItem != null) hoveredWeaponItem.HoverTextOff();
            hoveredWeaponItem = null;
            hoveredWeapon = collider.gameObject.transform.parent.gameObject;
            hoveredWeaponItem = hoveredWeapon.GetComponent<Weapon>();
            hoveredWeaponItem.HoverTextOn(hoveredWeaponItem.WeaponName);
            //Debug.Log(hoveredWeaponItem.WeaponName);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "Item") {
            if (hoveredWeaponItem != null) hoveredWeaponItem.HoverTextOff();
            hoveredWeaponItem = null;
            
        }
    }

    void TurnOffAnimation() {
        mainSlot.transform.GetChild(1).gameObject.SetActive(false);
    }

    //GETTERS
    public global::System.Boolean IsAttacking { get => isAttacking; set => isAttacking = value; }
    public global::System.Boolean IsHurting { get => isHurting; set => isHurting = value; }
    public global::System.Boolean IsAnimationLocked { get => isAnimationLocked; set => isAnimationLocked = value; }
    public Weapon MainSlot { get => mainSlot; set => mainSlot = value; }
    public Weapon SubSlot { get => subSlot; set => subSlot = value; }
    public global::System.Int32 WeaponAnimationFrame { get => weaponAnimationFrame; set => weaponAnimationFrame = value; }
    public global::System.Int32 Glucose { get => glucose; set => glucose = value; }
    public global::System.Int32 HealthPotions { get => healthPotions; set => healthPotions = value; }
    public global::System.Int32 GlucosePotions { get => glucosePotions; set => glucosePotions = value; }
    public global::System.Int32 HPotTimer { get => hPotTimer; set => hPotTimer = value; }
    public global::System.Int32 HPotCooldown { get => hPotCooldown; set => hPotCooldown = value; }
    public global::System.Int32 GPotTimer { get => gPotTimer; set => gPotTimer = value; }
    public global::System.Int32 GPotCooldown { get => gPotCooldown; set => gPotCooldown = value; }
    public global::System.Int32 Health { get => health; set => health = value; }
    public global::System.Int32 Glucose1 { get => glucose; set => glucose = value; }
    public global::System.Int32 Yeast { get => yeast; set => yeast = value; }
    public global::System.Int32 YeastLevel { get => yeastLevel; set => yeastLevel = value; }
    public global::System.Int32 MaxHealth { get => maxHealth; set => maxHealth = value; }
    public global::System.Int32 MaxGlucose { get => maxGlucose; set => maxGlucose = value; }
    public global::System.Int32 MaxYeast { get => maxYeast; set => maxYeast = value; }
    public global::System.Int32 MaxYeastLevel { get => maxYeastLevel; set => maxYeastLevel = value; }
    public Weapon MainSlot1 { get => mainSlot; set => mainSlot = value; }
    public Weapon SubSlot1 { get => subSlot; set => subSlot = value; }
    public Passive PassiveSlot { get => passiveSlot; set => passiveSlot = value; }
    public global::System.Int32 Gold { get => gold; set => gold = value; }
    public global::System.Boolean IsNavigatingUI { get => isNavigatingUI; set => isNavigatingUI = value; }
    public SaveData CurSaveData { get => curSaveData; set => curSaveData = value; }
    public global::System.Boolean IsLocked { get => isLocked; set => isLocked = value; }
}
