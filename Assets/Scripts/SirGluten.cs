using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SirGluten : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float verticalInput, horizontalInput;
    private float speed = 4f;
    

    // STATS
    private int health, glucose, yeast, yeastLevel;
    private int maxHealth=100, maxGlucose=30, maxYeast, maxYeastLevel = 10;

    [SerializeField] private GameObject healthBar, glucoseBar, yeastBar;
    private Slider healthSlider, glucoseSlider, yeastSlider;
    private TMPro.TextMeshProUGUI healthText, glucoseText, yeastText;

    [SerializeField] private GameObject damagePopup;

    // BOOLS
    private bool isAttacking, isHurting, isLocked, isAnimationLocked;

    // INVENTORY
    private GameObject hoveredWeapon;
    private Item hoveredWeaponItem;
    private Item mainSlot,subSlot;

    [SerializeField]private Image mainSlotImage, subSlotImage;

    // STATICS
    public static Vector2 playerPosition;

    private int weaponAnimationFrame = 0;
    public static int staticYeast = 0;

    //GETTERS

    public global::System.Boolean IsAttacking { get => isAttacking; set => isAttacking = value; }
    public global::System.Boolean IsHurting { get => isHurting; set => isHurting = value; }
    public global::System.Boolean IsAnimationLocked { get => isAnimationLocked; set => isAnimationLocked = value; }
    public Item MainSlot { get => mainSlot; set => mainSlot = value; }
    public Item SubSlot { get => subSlot; set => subSlot = value; }
    public global::System.Int32 WeaponAnimationFrame { get => weaponAnimationFrame; set => weaponAnimationFrame = value; }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        InitStats();
        UpdateStats();
    }

    // Initializes the Stat Values and Slider/Text variables.
    void InitStats(){
        health = maxHealth;
        glucose = maxGlucose;
        yeast = 0;
        yeastLevel = 1;

        healthSlider = healthBar.GetComponentInChildren<Slider>();
        glucoseSlider = glucoseBar.GetComponentInChildren<Slider>();
        yeastSlider = yeastBar.GetComponentInChildren<Slider>();

        healthText = healthBar.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        glucoseText = glucoseBar.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        yeastText = yeastBar.GetComponentInChildren<TMPro.TextMeshProUGUI>();
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
        

        healthSlider.value = health;
        healthSlider.maxValue = maxHealth;
        glucoseSlider.value = glucose;
        glucoseSlider.maxValue = maxGlucose;
        yeastSlider.value = yeast;
        yeastSlider.maxValue = maxYeast;


        healthText.text = health + "/" + maxHealth;
        glucoseText.text = glucose + "/" + maxGlucose;
        if (yeastLevel >= maxYeastLevel) {
            yeast = maxYeast;
            yeastText.text = "Level: MAX";
        } else {
            yeastText.text = "Level: " + yeastLevel;
        }
        
    }

    void Update() {
        // Statics
        playerPosition = body.position;

        // Stats
        UpdateStats();

        if (health <= 0) { 
            SceneManager.LoadScene(1);
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

        if (!isLocked) MovePlayer();    

        // ANimations
        if (!isAnimationLocked) {
            animator.SetFloat("horizontal", Mathf.Abs(body.linearVelocity.x));
            animator.SetFloat("vertical", body.linearVelocity.y);
            animator.SetBool("isMoving", body.linearVelocity.x != 0 || body.linearVelocity.y != 0);
        }
    }

    void UpdateInventory() {
        if (mainSlot != null) {
            mainSlotImage.sprite = mainSlot.InventorySprite;
            mainSlotImage.color = Color.white;
        } else {
            mainSlotImage.sprite = null;
            mainSlotImage.color = new Color(1f,1f,1f,0f);
        }
        if (subSlot != null) {
            subSlotImage.sprite = subSlot.InventorySprite;
            subSlotImage.color = Color.white;
        } 
        else {
            subSlotImage.sprite = null;
            subSlotImage.color = new Color(1f,1f,1f,0f);
        }
    }

    void Equip() {
        if (mainSlot != null && subSlot == null) {
            subSlot = mainSlot;
            subSlot.IsEquipped = false;
            subSlotImage.sprite = mainSlot.InventorySprite;
        }

        mainSlot = hoveredWeaponItem;
        mainSlot.IsEquipped = true;


        GameObject hoveredWeaponDropped = hoveredWeapon.transform.GetChild(0).gameObject;
        
    
        hoveredWeaponDropped.SetActive(false);
        hoveredWeapon.transform.SetParent(transform);   

        mainSlot.transform.position = body.position;
        mainSlot.transform.localRotation = Quaternion.identity;


        UpdateInventory();
    }

    void Swap() {
        if (subSlot == null) return;

        Item tempSlot = subSlot;
        subSlot = mainSlot;
        mainSlot = tempSlot;
        mainSlot.IsEquipped = true;
        subSlot.IsEquipped = false;

        UpdateInventory();
    }

    void Drop() {
        if (mainSlot == null) return;

        GameObject items = GameObject.Find("ItemStore");
        GameObject itemDropped = hoveredWeapon.transform.GetChild(0).gameObject;

        if (items != null && itemDropped != null) {
            mainSlot.IsEquipped = false;
            RectTransform mainSlotRect = mainSlot.GetComponent<RectTransform>();

            if (mainSlotRect != null) {
                mainSlot.transform.SetParent(items.transform);
                mainSlotRect.rotation = Quaternion.identity;
                mainSlotRect.anchoredPosition = body.position;
            }
            itemDropped = mainSlot.transform.GetChild(0).gameObject;
            
            itemDropped.gameObject.SetActive(true);

            mainSlot = null;
        } else {
            return;
        }

        if (subSlot != null) {
            mainSlot = subSlot;
            mainSlot.IsEquipped = true;
            subSlot = null;
            
        }
        
        UpdateInventory();
    }

    IEnumerator Hurt(int damage){
        if (isHurting) yield break;
        isHurting = true;

        health -= damage;
        CreatePopup(damage);

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
        dp.transform.SetParent(GameManager.PopupStore.transform);
    }

    void MovePlayer(){
        body.linearVelocity = new Vector2(horizontalInput,verticalInput).normalized * speed;
        
        if (!isAttacking) {
            if (horizontalInput < 0) {
                Vector2 rotator = new Vector3(transform.rotation.x, 180f);
                transform.rotation = Quaternion.Euler(rotator);
            } else if (horizontalInput > 0) {
                Vector2 rotator = new Vector3(transform.rotation.x, 0f);
                transform.rotation = Quaternion.Euler(rotator);
            }
            if (verticalInput != 0 && horizontalInput == 0) {
                Vector2 rotator = new Vector3(transform.rotation.x, 0f);
                transform.rotation = Quaternion.Euler(rotator);
            }
        
        }  
    }

    void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.gameObject.tag == "Enemy") {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
//            Debug.Log("You take damage: " + enemy.Damage + "Current damage: " + health);
            Vector2 collisionPoint = collision.transform.position;
                Vector2 direction;

            if (collisionPoint.x > transform.position.x) {
                direction = new Vector2(-1f, 1f); 
            } else {
                direction = new Vector2(1f, 1f);
            }

            isLocked = true;
            body.AddForce(direction * 1.2f, ForceMode2D.Impulse);
            StartCoroutine(Hurt(enemy.Damage));
        } else if (collision.gameObject.tag == "EnemyAttack") {
            EnemyAttack enemyAttack = collision.gameObject.GetComponent<EnemyAttack>();
            StartCoroutine(Hurt(enemyAttack.Damage));
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Item") {
            if (hoveredWeaponItem != null) hoveredWeaponItem.HoverTextOff();
            hoveredWeaponItem = null;
            hoveredWeapon = collider.gameObject.transform.parent.gameObject;
            hoveredWeaponItem = hoveredWeapon.GetComponent<Item>();
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

    void SetAnimationFrame(int index) {
        //Debug.Log(index);
        weaponAnimationFrame = index;
    }
}
