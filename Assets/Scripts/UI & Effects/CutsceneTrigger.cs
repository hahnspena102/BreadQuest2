using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private string destinationScene;
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.transform.tag == "Player") {
            SceneManager.LoadScene(destinationScene);
            Destroy(gameObject,0.1f);
        }

                    
    }
}
