using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    private SirGluten sirGluten;
    private Coroutine dialogueCoroutine;
    [SerializeField]private CanvasGroup canvasGroup;
    [SerializeField]private TextMeshProUGUI textBox;
    [SerializeField]private Image icon;

    [SerializeField] private List<DialogueEntry> dialogueEntries = new List<DialogueEntry>();
    private List<Sprite> currentIcons =  new List<Sprite>();
    private float elapsedTime;
    private int iconIndex = 0;

    void Start(){
        canvasGroup.alpha = 0;
    }

    void Update(){
        if (dialogueCoroutine == null && dialogueEntries.Count > 0) {
            dialogueCoroutine = StartCoroutine(ProcessDialogue());
        }

        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.5f && currentIcons.Count > 0) {
            icon.sprite = currentIcons[iconIndex];
            iconIndex += 1;
            if (iconIndex >= currentIcons.Count) iconIndex = 0;
            elapsedTime = 0;
        }
    }

    IEnumerator ProcessDialogue(){
        while (dialogueEntries.Count > 0) {
            DialogueEntry currentEntry = dialogueEntries[0];

            Debug.Log(currentEntry.Text);

            // Initialize
            canvasGroup.alpha = 1f;
            textBox.text = "";

            // Sprites
            iconIndex = 0;
            if (currentEntry.IconSprites.Count > 0) {
                icon.sprite = currentEntry.IconSprites[0];
                currentIcons = currentEntry.IconSprites;
            }
            yield return new WaitForSeconds(0.5f);

            // CHAR BY CHAR
            string fullText = currentEntry.Text;
            foreach (char letter in fullText)
            {
                textBox.text += letter;
                yield return new WaitForSeconds(0.02f);
            }

            yield return new WaitForSeconds(2f);
            

            // Canvas Group Fade Out
            float fadeDuration = 1f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
                canvasGroup.alpha = alpha;
                yield return null;
            }
            
            dialogueEntries.RemoveAt(0);
            yield return new WaitForSeconds(0.5f);
        }
        
    }

    public void UpdateDialogue(List<DialogueEntry> newEntries) {
        dialogueEntries = new List<DialogueEntry>();
        dialogueEntries = newEntries;
        if (dialogueCoroutine != null) StopCoroutine(dialogueCoroutine);
        dialogueCoroutine = StartCoroutine(ProcessDialogue());
    }
}