using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SirGluten : MonoBehaviour
{
    private Rigidbody2D body;
    private float verticalInput, horizontalInput;
    private float speed = 4f;

    // STATS
    private int health, glucose, yeast, yeastLevel;
    private int maxHealth=100, maxGlucose=100, maxYeast, maxYeastLevel;

    [SerializeField] private GameObject healthBar, glucoseBar, yeastBar;
    private Slider healthSlider, glucoseSlider, yeastSlider;
    private TMPro.TextMeshProUGUI healthText, glucoseText, yeastText;

    // INVENTORY
    private GameObject hoveredWeapon;
    private Item hoveredWeaponItem;
    private Item mainSlot,subSlot;

    [SerializeField]private Image mainSlotImage, subSlotImage;
    
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

        

    }
    void FixedUpdate()
    {
        //MovePlayer();
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
        
        if (horizontalInput < 0) {
            Vector2 rotator = new Vector3(transform.rotation.x, 180f);
            transform.rotation = Quaternion.Euler(rotator);
        } else if (horizontalInput > 0) {
            Vector2 rotator = new Vector3(transform.rotation.x, 0f);
            transform.rotation = Quaternion.Euler(rotator);
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
