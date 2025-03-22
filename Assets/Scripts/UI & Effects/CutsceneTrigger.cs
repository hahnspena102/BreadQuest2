using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private string destinationScene;
    private SirGluten sirGluten;
    
    void Start(){
        GameObject sir = GameObject.Find("SirGluten");
        sirGluten = sir.GetComponent<SirGluten>();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.transform.tag == "Player") {
            StartCoroutine(NextFloor());
        }
    }

    IEnumerator NextFloor(){
        bool isSuccess = false;
        isSuccess = sirGluten.SaveData();
        while (!isSuccess) {
            isSuccess = sirGluten.SaveData();
        }
        Rigidbody2D rb = sirGluten.GetComponent<Rigidbody2D>();          
        if (rb != null) rb.constraints = RigidbodyConstraints2D.FreezeAll; 
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(destinationScene);
    }
}
