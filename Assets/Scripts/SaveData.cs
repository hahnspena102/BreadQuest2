using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Scriptable Objects/SaveData")]
public class SaveData : ScriptableObject
{
    [SerializeField]private int yeast;
    [SerializeField]private int yeastLevel;
    [SerializeField]private int gold;
    [SerializeField]private int healthPotions;
    [SerializeField]private int glucosePotions;
    [SerializeField]private string mainSlotId;
    [SerializeField]private string subSlotId;
    [SerializeField]private Passive passive;
    [SerializeField]private string floor;

    public global::System.Int32 Yeast { get => yeast; set => yeast = value; }
    public global::System.Int32 YeastLevel { get => yeastLevel; set => yeastLevel = value; }
    public global::System.String MainSlotId { get => mainSlotId; set => mainSlotId = value; }
    public global::System.String SubSlotId { get => subSlotId; set => subSlotId = value; }
    public Passive Passive { get => passive; set => passive = value; }
    public global::System.Int32 HealthPotions { get => healthPotions; set => healthPotions = value; }
    public global::System.Int32 GlucosePotions { get => glucosePotions; set => glucosePotions = value; }
    public global::System.Int32 Gold { get => gold; set => gold = value; }
    public global::System.String Floor { get => floor; set => floor = value; }

    public void ResetData(){
        yeast = 0;
        yeastLevel = 1;
        gold = 0;
        mainSlotId = "";
        subSlotId = null;
        passive = null;
        healthPotions = 0;
        glucosePotions = 0;
        floor = "Floor1";
    }

    public void TutorialData(){
        yeast = 0;
        yeastLevel = 1;
        gold = 100;
        mainSlotId = "";
        subSlotId = null;
        passive = null;
        healthPotions = 3;
        glucosePotions = 3;
        floor = "Tutorial";
    }
}
