using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour 
{
    [SerializeField] private GameObject infoUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject compendium;
    [SerializeField] private GameObject floorIntro;
    [SerializeField] private List<Font> fonts = new List<Font>();
    private SirGluten sirGluten;

    private CanvasGroup floorIntroCanvasGroup;
    [SerializeField]private TextMeshProUGUI floorIntroText;

    void Start() {
        foreach (Font f in fonts) {
            f.material.mainTexture.filterMode = FilterMode.Point;
        }

        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();

        floorIntroCanvasGroup = floorIntro.GetComponent<CanvasGroup>();
        if (sirGluten != null) {
            if (sirGluten.CurSaveData.Floor.StartsWith("Floor")) {
                floorIntroText.text = "Floor " + sirGluten.CurSaveData.Floor.Substring(5);
            } else {
                floorIntroText.text = sirGluten.CurSaveData.Floor;
            }
        }

        floorIntro.SetActive(true);
        StartCoroutine(FadeFloorIntro(1f));
    }

    void Update() {
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

    public void ActivateShop(Statue statue, List<Passive> passivesSold) {
        shopUI.SetActive(true);
        shopUI.GetComponent<ShopUI>().CurStatue = statue;

        int i = 0;
        foreach (Passive p in passivesSold) {
            shopUI.GetComponent<ShopUI>().UpdateOffer(i, p);
            i++;
        }
    }

    public void DeactivateShop() {
        shopUI.SetActive(false);
    }


    public IEnumerator FadeFloorIntro(float fadeDuration) {
        yield return new WaitForSeconds(1f);
        float startAlpha = floorIntroCanvasGroup.alpha;

        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration) {
            timeElapsed += Time.deltaTime;
            floorIntroCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, timeElapsed / fadeDuration);
            yield return null;
        }

        
        floorIntroCanvasGroup.alpha = 0f;
        floorIntro.SetActive(false);
    }

}
