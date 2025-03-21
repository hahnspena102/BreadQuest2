using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueBox dialogueBox;
    [SerializeField] private List<DialogueEntry> entries = new List<DialogueEntry>();

    void Start()
    {   
        dialogueBox = GameObject.Find("DialogueBox").GetComponent<DialogueBox>();
        if (dialogueBox == null) {
            Debug.Log("Could not find dialogue box.");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.transform.tag == "Player") {
            if (entries != null) dialogueBox.UpdateDialogue(entries);
            Destroy(gameObject,0.1f);
        }
    }
}
