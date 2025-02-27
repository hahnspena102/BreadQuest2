using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SirGluten : MonoBehaviour
{
    private Vector2 screenSize = new Vector2(Screen.width/2, Screen.height/2);
    private Rigidbody2D body;
    private Vector2 mousePosition;
    private float verticalInput, horizontalInput;
    private float speed = 4f;
    

    // STATS
    private int health, glucose, yeast, yeastLevel;
    private int maxHealth=100, maxGlucose=100, maxYeast, maxYeastLevel;

    [SerializeField] private GameObject healthBar, glucoseBar, yeastBar;
    private Slider healthSlider, glucoseSlider, yeastSlider;
    private TMPro.TextMeshProUGUI healthText, glucoseText, yeastText;

    // BOOLS
    private bool isAttacking = false;

    // INVENTORY
    private GameObject hoveredWeapon;
    private Item hoveredWeaponItem;
    private Item mainSlot,subSlot;

    [SerializeField]private Image mainSlotImage, subSlotImage;

    // STATICS
    public static Vector2 playerPosition;
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
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
        healthSlider.value = health;
        glucoseSlider.value = glucose;
        yeastSlider.value = yeast;

        healthText.text = health + "/" + maxHealth;
        glucoseText.text = glucose + "/" + maxGlucose;
        yeastText.text = "Level: " + yeastLevel;
    }

    void Update() {
        // Statics
        playerPosition = body.position;
        
        // Stats
        healthSlider.value = health;
        healthSlider.maxValue = maxHealth;
        healthText.text = $"{health}/{maxHealth}";

        // Movement
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.E) && hoveredWeaponItem != null) {
            Equip();
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            Swap();
        }
        if (Input.GetKeyDown(KeyCode.Q) && mainSlot != null) {
            Drop();
        }

        MovePlayer();

        MeleeDirection();
        if (Input.GetMouseButtonDown(0) && mainSlot != null && !isAttacking) {
            StartCoroutine(MeleeAttack());
        }
        
        
        
    }

    private void MeleeDirection() {
        if (!isAttacking) {
            mousePosition = (Vector2)Input.mousePosition - screenSize;
        }
        if (mainSlot != null) {
            if ((mousePosition.y != 0 || mousePosition.x != 0)) {
                Rigidbody2D attackRB = mainSlot.transform.GetChild(1).gameObject.GetComponent<Rigidbody2D>();
                if (Mathf.Abs(mousePosition.x) > Mathf.Abs(mousePosition.y)) {
                    attackRB.position = body.position + new Vector2(mousePosition.x, 0).normalized;
                } else {
                    attackRB.position = body.position + new Vector2(0, mousePosition.y).normalized;
                }
                
            }
        }
    }

    IEnumerator MeleeAttack() {
        isAttacking = true;
        mainSlot.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
        mainSlot.transform.GetChild(1).gameObject.SetActive(false);
        
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
            subSlotImage.sprite = mainSlot.InventorySprite;
        }

        mainSlot = hoveredWeaponItem;

        GameObject hoveredWeaponDropped = hoveredWeapon.transform.GetChild(0).gameObject;
        hoveredWeaponDropped.SetActive(false);

        hoveredWeapon.transform.SetParent(transform);     
        UpdateInventory();
    }

    void Swap() {
        if (subSlot == null) return;

        Item tempSlot = subSlot;
        subSlot = mainSlot;
        mainSlot = tempSlot;

        UpdateInventory();
    }

    void Drop() {
        if (mainSlot == null) return;

        GameObject items = GameObject.Find("Items");
        GameObject itemDropped = hoveredWeapon.transform.GetChild(0).gameObject;

        if (items != null && itemDropped != null) {
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
            subSlot = null;
            
        }
        
        UpdateInventory();
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
        }  
    }

    void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.gameObject.tag == "Enemy") {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
//            Debug.Log("You take damage: " + enemy.Damage + "Current damage: " + health);
        
            health -= enemy.Damage;
            
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
}
