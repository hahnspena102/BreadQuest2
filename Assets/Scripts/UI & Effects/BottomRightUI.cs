using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class BottomRightUI : MonoBehaviour
{
    private SirGluten sirGluten;

    [SerializeField] private TextMeshProUGUI hPotCount, hPotCooldown;
    [SerializeField] private TextMeshProUGUI gPotCount, gPotCooldown;

    void Start(){
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
    }

    void Update(){
        hPotCount.text = $"{sirGluten.HealthPotions}";
        hPotCooldown.text = $"{sirGluten.HPotTimer}";
        gPotCount.text = $"{sirGluten.GlucosePotions}";
        gPotCooldown.text = $"{sirGluten.GPotTimer}";
    }
}