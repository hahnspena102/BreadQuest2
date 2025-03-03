using System.Collections;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private float moveSpeed = 0.5f;
    private float fadeDuration = 1f;
    private TextMeshPro textMesh;
    private Color originalColor;
    private int damageNumber;

    public global::System.Int32 DamageNumber { get => damageNumber; set => damageNumber = value; }

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        originalColor = textMesh.color;
        StartCoroutine(FadeOutAndMoveUp());
        textMesh.text = $"{damageNumber}";
    }

    private IEnumerator FadeOutAndMoveUp()
    {
        float elapsedTime = 0f;
        Vector2 startPosition = transform.position;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            transform.position = startPosition + new Vector2(0, moveSpeed * (elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}