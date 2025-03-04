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
    private Color outlineColor = Color.black;
    private bool isCritical, isPlayerHurt;

    public global::System.Int32 DamageNumber { get => damageNumber; set => damageNumber = value; }
    public Color OutlineColor { get => outlineColor; set => outlineColor = value; }
    public global::System.Boolean IsCritical { get => isCritical; set => isCritical = value; }
    public global::System.Boolean IsPlayerHurt { get => isPlayerHurt; set => isPlayerHurt = value; }

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        if (isCritical) {
            textMesh.color = Color.yellow;
            textMesh.fontSize = 6;
        }
        if (IsPlayerHurt) {
            textMesh.color = new Color(255/255f, 110/255f,110/255f);
        }
        
        originalColor = textMesh.color;
        StartCoroutine(FadeOutAndMoveUp());
        textMesh.text = $"{damageNumber}";

        textMesh.fontMaterial.SetColor("_OutlineColor", outlineColor);
        //textMesh.fontMaterial.SetFloat("_OutlineWidth", 1f);
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