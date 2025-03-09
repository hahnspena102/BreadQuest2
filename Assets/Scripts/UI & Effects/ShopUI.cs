using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopUI : MonoBehaviour
{
    private SirGluten sirGluten;
    
    [SerializeField]private List<GameObject> passiveOffer = new List<GameObject>();
    [SerializeField]private List<Passive> curPassives = new List<Passive>(){null, null, null};

    private void Start(){
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
    }

    public void UpdateOffer(int index, Passive passive) {
        if (index < 0 || index > passiveOffer.Count) {
            return;
        }
        TextMeshProUGUI name = passiveOffer[index].transform.GetChild(0).gameObject.transform.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI description = passiveOffer[index].transform.GetChild(1).gameObject.transform.GetComponent<TextMeshProUGUI>();
        Button button = passiveOffer[index].transform.GetChild(2).gameObject.transform.GetComponent<Button>();
        TextMeshProUGUI cost = passiveOffer[index].transform.GetChild(3).gameObject.transform.GetComponent<TextMeshProUGUI>();
        Image img = passiveOffer[index].transform.GetChild(4).gameObject.transform.GetComponent<Image>();

        name.text = passive.PassiveName;
        description.text = passive.Description;
        cost.text = passive.GoldCost + " Gold";
        img.sprite = passive.Sprite;

        curPassives[index] = passive;
    }

    public void BuyPassive(int index) {
        sirGluten.PassiveSlot = curPassives[index];
    }

    public void BuyHPot() {
        sirGluten.HealthPotions += 1;
    }

    public void BuyGPot() {
        sirGluten.GlucosePotions += 1;
    }
}