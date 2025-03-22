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
    [SerializeField]private bool saveAfterTeleport = true;


    void Start(){
        hoverText = transform.GetChild(0).gameObject;

            sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E)) {
            StartCoroutine(NextFloor());
        }
    }

    IEnumerator NextFloor(){
        bool isSuccess = true;
        if (saveAfterTeleport) {
            isSuccess = sirGluten.SaveData();
            while (!isSuccess) {
                isSuccess = sirGluten.SaveData();
            }
            Rigidbody2D rb = sirGluten.GetComponent<Rigidbody2D>();          
            if (rb != null) rb.constraints = RigidbodyConstraints2D.FreezeAll; 
            SpriteRenderer sp = sirGluten.GetComponent<SpriteRenderer>();
            if (sp != null) sp.color = new Color(1f,1f,1f,0f);
            yield return new WaitForSeconds(2f);

        }
        
        SceneManager.LoadScene(destinationScene);
    }

    void OnTriggerStay2D(Collider2D collider) {
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
