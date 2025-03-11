using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]private SaveData saveData;
    [SerializeField]private SaveData tutorialData;
    [SerializeField]private Button continueButton;
    void Update() {
        continueButton.interactable = saveData.Floor != "Floor1" && saveData.Floor != "";
    }
    public void NewGame() {
        saveData.ResetData();
        SceneManager.LoadScene("Floor1");
    }

    public void ContinueGame() {
        SceneManager.LoadScene(saveData.Floor);
    }

    public void Tutorial() {
        tutorialData.TutorialData();
        SceneManager.LoadScene("Tutorial");
    }
}
