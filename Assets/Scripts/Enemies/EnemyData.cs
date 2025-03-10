using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField]private string enemyName;
    [SerializeField]private string description;
    [SerializeField]private int health;
    [SerializeField]private int damage;
    [SerializeField]private string flavoring;
    [SerializeField]private float xpMultiplier = 1f;
    [SerializeField]private float goldMultiplier = 1f;
    [SerializeField]private Sprite sprite;
    [SerializeField]private Color color = Color.white;
    [SerializeField]private bool isDiscovered;
    [SerializeField] private float detectionRadius = 10f;

    public global::System.Int32 Health { get => health; set => health = value; }
    public global::System.Int32 Damage { get => damage; set => damage = value; }
    public global::System.String Flavoring { get => flavoring; set => flavoring = value; }
    public global::System.Single XpMultiplier { get => xpMultiplier; set => xpMultiplier = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public Color Color { get => color; set => color = value; }
    public global::System.String EnemyName { get => enemyName; set => enemyName = value; }
    public global::System.String Description { get => description; set => description = value; }
    public global::System.Boolean IsDiscovered { get => isDiscovered; set => isDiscovered = value; }
    public global::System.Single DetectionRadius { get => detectionRadius; set => detectionRadius = value; }
    public global::System.Single GoldMultiplier { get => goldMultiplier; set => goldMultiplier = value; }
}
