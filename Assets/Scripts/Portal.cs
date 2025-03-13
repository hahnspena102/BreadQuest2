using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private GameObject hoverText;
    private bool isInteractable;
    private SirGluten sirGluten;
    [SerializeField]private string destinationScene;

    void Start(){
        hoverText = transform.GetChild(0).gameObject;

            sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E)) {
            sirGluten.CurSaveData.Floor = destinationScene;
            NextFloor();
        }
    }

    void NextFloor(){
        sirGluten.SaveData();
        SceneManager.LoadScene(destinationScene);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.transform.tag == "Player") {
            hoverText.SetActive(true);
            isInteractable = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.transform.tag == "Player") {
            hoverText.SetActive(false);
            isInteractable = false;
        }
    }
}
