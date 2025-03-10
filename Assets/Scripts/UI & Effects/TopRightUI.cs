using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class TopRightUI : MonoBehaviour
{
    private SirGluten sirGluten;
 
    [SerializeField] private Slider yeastSlider;
    [SerializeField] private TextMeshProUGUI yeastText;
    [SerializeField] private TextMeshProUGUI goldText;

    void Start(){
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
    }

    void Update(){  
        yeastSlider.value = sirGluten.Yeast;
        yeastSlider.maxValue = sirGluten.MaxYeast;
        goldText.text = sirGluten.Gold + " G";

        if (sirGluten.YeastLevel >= sirGluten.MaxYeastLevel) {
            yeastText.text = "Level: MAX";
        } else {
            yeastText.text = "Level: " + sirGluten.YeastLevel;
        }
    }
}