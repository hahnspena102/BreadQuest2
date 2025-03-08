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

    private SpriteRenderer spriteRenderer;

    public global::System.Int32 AttackDamage { get => attackDamage; set => attackDamage = value; }
    public global::System.String Flavor { get => flavor; set => flavor = value; }

    void Start(){
        Rigidbody2D rb= GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (rb != null) {
            rb.linearVelocity = rb.linearVelocity * speed; 
        }

        StartCoroutine(FadeOutAndDestroy());
    }
    void OnCollisionEnter2D(Collision2D collision) {
        if (isPassable) return;
        if (collision.gameObject.tag == "Enemy") {
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "Walls") {
            Destroy(gameObject);
        }
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