using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class GameOver : MonoBehaviour
{
    [SerializeField]private TMPro.TextMeshProUGUI textBox;
    [SerializeField]private CanvasGroup continueGroup;
    [SerializeField]private CanvasGroup black;
    [SerializeField]private SaveData playerData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textBox.text = "";
        StartCoroutine(GameOverCo());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            try {
                SceneManager.LoadScene(playerData.Floor);
            }
            catch (Exception e) {
                Debug.Log(e);
                SceneManager.LoadScene("Floor1");
            }  
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator GameOverCo() {
        float fadeDuration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            black.alpha = alpha;
            yield return null;
        }

        string fullText = "Game Over!";
        
           foreach (char letter in fullText)
            {
                textBox.text += letter;
                yield return new WaitForSeconds(0.1f);
            }

        yield return new WaitForSeconds(1f);

        fadeDuration = 5f;
        elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            continueGroup.alpha = alpha;
            yield return null;
        }
    }
}
