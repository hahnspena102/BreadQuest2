using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Dialogue Entry", menuName = "Scriptable Objects/Dialogue Entry")]
public class DialogueEntry : ScriptableObject
{
    [SerializeField]private List<Sprite> iconSprites;
    [SerializeField]private string text;

    public List<Sprite> IconSprites { get => iconSprites; set => iconSprites = value; }
    public global::System.String Text { get => text; set => text = value; }
}
