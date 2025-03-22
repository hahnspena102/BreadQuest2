using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class Compendium : MonoBehaviour
{
    private SirGluten sirGluten;
    
    [SerializeField]private List<EnemyData> enemyData = new List<EnemyData>();
    [SerializeField]private TextMeshProUGUI enemyName, enemyDescription, pageText;
    [SerializeField]private Image enemyImage;
    [SerializeField]private Slider pageNavigator;
    private int pageNumber, maxPageNumber;


    private void Start(){
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();

        maxPageNumber = enemyData.Count - 1;
        pageNavigator.maxValue = maxPageNumber;
    }

    void Update() {
        EnemyData curEnemyData = enemyData[pageNumber];
        if (curEnemyData.IsDiscovered) {
            enemyName.text = curEnemyData.EnemyName;
            enemyDescription.text = $"Flavor: {curEnemyData.Flavoring}\nHP: {curEnemyData.Health}\n\n{curEnemyData.Description}";
            enemyImage.sprite = curEnemyData.Sprite;
            enemyImage.color = curEnemyData.Color;
        } else {
            enemyName.text = "???";
            enemyDescription.text = $"To be discovered...";
            enemyImage.sprite = curEnemyData.Sprite;
            enemyImage.color = Color.black;
            if (pageNumber == maxPageNumber) enemyImage.color = new Color(1f,1f,1f,0f);
        }
        
        pageText.text = $"Page {pageNumber + 1} of {maxPageNumber + 1}";
        pageNumber = (int)Mathf.Round(pageNavigator.value);

        if (Input.GetKeyDown(KeyCode.M)) ResetCompendium();
        if (Input.GetKeyDown(KeyCode.N)) FinishCompendium();

    }

    public void NextPage() {
        if (pageNumber >= maxPageNumber) return;
        pageNumber++;
        pageNavigator.value = pageNumber;
    }

    public void PrevPage(){
        if (pageNumber <= 0) return;
        pageNumber--;
        pageNavigator.value = pageNumber;
    }

    public void ResetCompendium(){
        foreach (EnemyData enemy in enemyData) enemy.IsDiscovered = false;
    }

    public void FinishCompendium(){
        foreach (EnemyData enemy in enemyData) enemy.IsDiscovered = true;
    }


}