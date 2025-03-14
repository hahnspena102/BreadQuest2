using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MagicProj : MonoBehaviour
{
    private int attackDamage;
    private string flavor;
    [SerializeField] private float speed;
    [SerializeField] private float duration = 5f;
    [SerializeField] private bool isPassable = false;
    [SerializeField] private float fadeDuration = 1f;

    [SerializeField]private float timeTilCollision = 0.1f;

    private float elapsedTime;

    private SpriteRenderer spriteRenderer;

    public global::System.Int32 AttackDamage { get => attackDamage; set => attackDamage = value; }
    public global::System.String Flavor { get => flavor; set => flavor = value; }

    void Start(){
        Rigidbody2D rb= GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (rb != null && rb.bodyType != RigidbodyType2D.Static) {
            rb.linearVelocity = rb.linearVelocity * speed; 
        }

        StartCoroutine(FadeOutAndDestroy());
    }

    void Update(){
        elapsedTime += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (isPassable || elapsedTime <= timeTilCollision) return;
        Destroy(gameObject);

    }

    private IEnumerator FadeOutAndDestroy() {
        yield return new WaitForSeconds(duration - fadeDuration);

        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}