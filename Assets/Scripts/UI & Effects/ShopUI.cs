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
    [SerializeField] private GameObject soldOut;
    [SerializeField] private TextMeshProUGUI hPotText, gPotText, hPotCostText, gPotCostText;
    [SerializeField] private Button hPotButton, gPotButton;

    private Statue curStatue;
    public Statue CurStatue { get => curStatue; set => curStatue = value; }

    private void Start(){
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
    }

    void Update() {
        if (curStatue != null) {
            hPotButton.interactable = curStatue.HPotStock > 0 && sirGluten.Gold > curStatue.PotionCost;
            gPotButton.interactable = curStatue.GPotStock > 0 && sirGluten.Gold > curStatue.PotionCost;
            hPotText.text = "Stock: " + curStatue.HPotStock;
            gPotText.text = "Stock: " + curStatue.GPotStock;
            hPotCostText.text = curStatue.PotionCost + " Gold";
            gPotCostText.text = curStatue.PotionCost + " Gold";

            for (int i = 0; i < passiveOffer.Count; i++) {
                Button button = passiveOffer[i].transform.GetChild(2).gameObject.transform.GetComponent<Button>();
                button.interactable = curPassives[i].GoldCost <= sirGluten.Gold;
            }
        }    
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
        sirGluten.Gold -= curPassives[index].GoldCost;
        Debug.Log(sirGluten.Gold);

        foreach (GameObject offer in passiveOffer) {
            offer.SetActive(false);
        }
        
        soldOut.SetActive(true);
    }

    public void BuyHPot() {
        if (sirGluten.Gold < curStatue.PotionCost) return;
        sirGluten.HealthPotions += 1;
        sirGluten.Gold -= curStatue.PotionCost;
        curStatue.HPotStock --;
    }

    public void BuyGPot() {
        if (sirGluten.Gold < curStatue.PotionCost) return;
        sirGluten.GlucosePotions += 1;
        sirGluten.Gold -= curStatue.PotionCost;
        curStatue.GPotStock--;
    }
}