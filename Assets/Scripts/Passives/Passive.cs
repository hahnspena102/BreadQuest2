using UnityEngine;

[CreateAssetMenu(fileName = "Passive", menuName = "Scriptable Objects/Passive")]
public class Passive : ScriptableObject
{
    [SerializeField]private string passiveName,description;
    [SerializeField]private Sprite sprite;
    [SerializeField]private int goldCost;

    // Attributes
    [SerializeField]private int healthRegeneration;

    public global::System.String PassiveName { get => passiveName; set => passiveName = value; }
    public global::System.String Description { get => description; set => description = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public global::System.Int32 HealthRegeneration { get => healthRegeneration; set => healthRegeneration = value; }
    public global::System.Int32 GoldCost { get => goldCost; set => goldCost = value; }
}
