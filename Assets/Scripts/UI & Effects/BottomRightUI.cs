using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class BottomRightUI : MonoBehaviour
{
    private SirGluten sirGluten;

    [SerializeField] private Image mainSlotImage, subSlotImage;

    void Start(){
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
    }

    void Update(){
        if (sirGluten.MainSlot != null) {
            mainSlotImage.sprite = sirGluten.MainSlot.InventorySprite;
            mainSlotImage.color = Color.white;
        } else {
            mainSlotImage.sprite = null;
            mainSlotImage.color = new Color(1f,1f,1f,0f);
        }
        if (sirGluten.SubSlot != null) {
            subSlotImage.sprite = sirGluten.SubSlot.InventorySprite;
            subSlotImage.color = Color.white;
        } 
        else {
            subSlotImage.sprite = null;
            subSlotImage.color = new Color(1f,1f,1f,0f);
        }
    }


}