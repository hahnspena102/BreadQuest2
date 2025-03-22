using UnityEngine;

[CreateAssetMenu(fileName = "Passive", menuName = "Scriptable Objects/Passive")]
public class Passive : ScriptableObject
{
    [SerializeField]private string passiveName,description;
    [SerializeField]private Sprite sprite;
    [SerializeField]private int goldCost;

    // Attributes (these work like bonuses that are added or subtracted)
    [SerializeField]private int healthRegeneration, glucoseRegeneration;
    [SerializeField]private float movementBonus;
    [SerializeField]private int defensiveBonus;
    [SerializeField]private float criticalBonus;

    public global::System.String PassiveName { get => passiveName; set => passiveName = value; }
    public global::System.String Description { get => description; set => description = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public global::System.Int32 GoldCost { get => goldCost; set => goldCost = value; }
    public global::System.Int32 HealthRegeneration { get => healthRegeneration; set => healthRegeneration = value; }
    public global::System.Int32 GlucoseRegeneration { get => glucoseRegeneration; set => glucoseRegeneration = value; }
    public global::System.Single MovementBonus { get => movementBonus; set => movementBonus = value; }
    public global::System.Int32 DefensiveBonus { get => defensiveBonus; set => defensiveBonus = value; }
    public global::System.Single CriticalBonus { get => criticalBonus; set => criticalBonus = value; }
}
