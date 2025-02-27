using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Item : MonoBehaviour
{
    [SerializeField]private string id;
    [SerializeField]private string weaponName;
    [SerializeField]private string description;
    private Sprite inventorySprite;

    public global::System.String Id { get => id; set => id = value; }
    public global::System.String WeaponName { get => weaponName; set => weaponName = value; }
    public global::System.String Description { get => description; set => description = value; }
    public Sprite InventorySprite { get => inventorySprite; set => inventorySprite = value; }

    void Start(){
        inventorySprite = GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public void HoverTextOn(string itemName) {
        GameObject hoverText = transform.Find("Dropped")?.gameObject.transform.Find("HoverText")?.gameObject;
        if (hoverText != null) {
            TMPro.TextMeshProUGUI hoverTextTMP = hoverText.GetComponent<TMPro.TextMeshProUGUI>();
            hoverTextTMP.text = "[E] " + itemName;

            hoverText.SetActive(true);
        }
    }

    public void HoverTextOff() {
        GameObject hoverText = transform.Find("Dropped")?.gameObject.transform.Find("HoverText")?.gameObject;
        if (hoverText != null) {
            hoverText.SetActive(false);
        }
    }
}
