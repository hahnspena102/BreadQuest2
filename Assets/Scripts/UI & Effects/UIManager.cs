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

    void Start(){
        foreach (Font f in fonts) {
            f.material.mainTexture.filterMode = FilterMode.Point;
        }
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
        
    }
    
}
