using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Cutscene Entry", menuName = "Scriptable Objects/Cutscene Entry")]
public class CutsceneEntry : ScriptableObject
{
    [SerializeField]private Sprite cutsceneFrame;
    [SerializeField]private string text;

    public global::System.String Text { get => text; set => text = value; }
    public Sprite CutsceneFrame { get => cutsceneFrame; set => cutsceneFrame = value; }
}
