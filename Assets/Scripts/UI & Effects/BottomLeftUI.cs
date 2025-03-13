using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class BottomLeftUI : MonoBehaviour
{
    private SirGluten sirGluten;

    [SerializeField] private TextMeshProUGUI hPotCount, hPotCooldown;
    [SerializeField] private TextMeshProUGUI gPotCount, gPotCooldown;
    [SerializeField] private Image hPotImage, gPotImage;

    [SerializeField] private Image passiveImage;

    void Start(){
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
    }

    void Update(){
        hPotCooldown.gameObject.SetActive(sirGluten.HPotTimer > 0);
        gPotCooldown.gameObject.SetActive(sirGluten.GPotTimer > 0);
        hPotCount.text = $"{sirGluten.HealthPotions}";
        hPotCooldown.text = $"{sirGluten.HPotTimer}";
        gPotCount.text = $"{sirGluten.GlucosePotions}";
        gPotCooldown.text = $"{sirGluten.GPotTimer}";

        if (sirGluten.HPotTimer > 0) {
            hPotImage.color = new Color(0.3605143f,0.3418921f,0.3679245f);
        } else {
            hPotImage.color = Color.white;
        }

        if (sirGluten.GPotTimer > 0) {
            gPotImage.color = new Color(0.3605143f,0.3418921f,0.3679245f);
        } else {
            gPotImage.color = Color.white;
        }

        if (sirGluten.PassiveSlot != null) {
            passiveImage.color = Color.white;
            passiveImage.sprite = sirGluten.PassiveSlot.Sprite;
        } else {    
            passiveImage.color = new Color(0f,0f,0f,0f);
            passiveImage.sprite = null;
        }
    }

    
}