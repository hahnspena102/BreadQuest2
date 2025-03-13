using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;


public class UIManager : MonoBehaviour 
{
    [SerializeField]private GameObject infoUI;
    [SerializeField]private GameObject shopUI;
    [SerializeField]private GameObject compendium;

    [SerializeField]private List<Font> fonts = new List<Font>();
    private SirGluten sirGluten;

    void Start(){
        foreach (Font f in fonts) {
            f.material.mainTexture.filterMode = FilterMode.Point;
        }

        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Z)) {
            if (compendium.activeSelf) {
                compendium.SetActive(false);
            } else {
                compendium.SetActive(true);
            }
        }

        if (Input.GetKey(KeyCode.Tab)) {
            infoUI.SetActive(true);
        } else {
            infoUI.SetActive(false);
        }

        // BOOLS
        sirGluten.IsNavigatingUI = shopUI.activeSelf || compendium.activeSelf;
        
    }

    public void ActivateShop(Statue statue, List<Passive> passivesSold){
        shopUI.SetActive(true);
        shopUI.GetComponent<ShopUI>().CurStatue = statue;

        int i = 0;
        foreach (Passive p in passivesSold) {
            shopUI.GetComponent<ShopUI>().UpdateOffer(i, p);
            i++;
        }
    }

    public void DeactivateShop(){
        shopUI.SetActive(false);
    }    
}
