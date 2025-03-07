using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour
{
    [SerializeField]private string id;
    [SerializeField]private string weaponName;
    [SerializeField]private string description;
    [SerializeField]private string itemType;
    [SerializeField]private string rarity;
    [SerializeField] private int attackDamage;
    [SerializeField] private string flavor;
    [SerializeField] private List<AudioClip> attackSFX;
    private Sprite inventorySprite;

    public global::System.String Id { get => id; set => id = value; }
    public global::System.String WeaponName { get => weaponName; set => weaponName = value; }
    public global::System.String Description { get => description; set => description = value; }
    public Sprite InventorySprite { get => inventorySprite; set => inventorySprite = value; }
    public global::System.Int32 AttackDamage { get => attackDamage; set => attackDamage = value; }
    public global::System.String Rarity { get => rarity; set => rarity = value; }
    public global::System.String Flavor { get => flavor; set => flavor = value; }
    public List<AudioClip> AttackSFX { get => attackSFX; set => attackSFX = value; }
    private string animationDirection;
    public global::System.String AnimationDirection { get => animationDirection; set => animationDirection = value; }

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
