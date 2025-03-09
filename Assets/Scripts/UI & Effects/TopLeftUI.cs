using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class TopLeftUI : MonoBehaviour
{
    private SirGluten sirGluten;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider glucoseSlider;
    [SerializeField] private TextMeshProUGUI glucoseText;

    void Start(){
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
    }

    void Update(){
        healthSlider.value = sirGluten.Health;
        healthSlider.maxValue = sirGluten.MaxHealth;
        glucoseSlider.value = sirGluten.Glucose;
        glucoseSlider.maxValue = sirGluten.MaxGlucose;


        healthText.text = sirGluten.Health + "/" + sirGluten.MaxHealth;
        glucoseText.text = sirGluten.Glucose + "/" + sirGluten.MaxGlucose;
    }
}