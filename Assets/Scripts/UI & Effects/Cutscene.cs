using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class Cutscene : MonoBehaviour
{

    [SerializeField]private List<CutsceneEntry> entries = new List<CutsceneEntry>();

    private int index = 0;
    private bool isDisplaying = false;
    [SerializeField] private string nextScene = "MainMenu";
    [SerializeField]private CanvasGroup currentFrame;
    [SerializeField]private CanvasGroup nextFrame;

    [SerializeField]private TextMeshProUGUI textBox;
    private Coroutine currentCoroutine;


    void Start()
    {
        index = 0;
        currentCoroutine = StartCoroutine(DisplayFrame());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (!isDisplaying) {
                index++;
                currentCoroutine = StartCoroutine(DisplayFrame());
            } else {
                if (currentCoroutine != null) {
                    StopCoroutine(currentCoroutine);
                    nextFrame.alpha = 1;
                    textBox.text = entries[index].Text + " [Enter]";
                    isDisplaying = false;
                }
            }
        }
    }

    IEnumerator DisplayFrame() {
        isDisplaying = true;
        if (index >= entries.Count) {
            SceneManager.LoadScene(nextScene);
            yield break;
        }

        // INIT
        Image currentImage = currentFrame.gameObject.GetComponent<Image>();
        Image nextImage = nextFrame.gameObject.GetComponent<Image>();

        currentImage.sprite = nextImage.sprite;
        nextImage.sprite = entries[index].CutsceneFrame;
        nextFrame.alpha = 0;
        textBox.text = "";
    
        float fadeDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            nextFrame.alpha = alpha;
            yield return null;
            
        }

        yield return new WaitForSeconds(0.5f);

        string fullText = entries[index].Text + " [Enter]";
        foreach (char letter in fullText)
        {
            textBox.text += letter;
            yield return new WaitForSeconds(0.04f);
        }

        isDisplaying = false;
    }
}
