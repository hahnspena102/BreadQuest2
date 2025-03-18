using UnityEngine;
using System.Collections;

public class PlayerProj : MonoBehaviour
{
    [SerializeField]private int attackDamage;
    [SerializeField]private string flavor;
    [SerializeField] private float speed;
    [SerializeField] private float duration = 5f;
    [SerializeField] private bool isPassable, isPiercing = false;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float timeTilCollision = 1f;
    [SerializeField]private int maxBounces = 0;
    private int bounces;

    private float elapsedTime = 0f;
    private SpriteRenderer spriteRenderer;
    private Collider2D hitbox;

    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public string Flavor { get => flavor; set => flavor = value; }

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb != null && rb.bodyType != RigidbodyType2D.Static)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }

        if (hitbox != null)
        {
            hitbox.enabled = false;
        }

        StartCoroutine(FadeOutAndDestroy());
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (hitbox != null && elapsedTime >= timeTilCollision)
        {
            hitbox.enabled = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPassable) return;
        if (collision.gameObject.tag == "Walls") {
            if (bounces < maxBounces) {
            bounces += 1;
            return;
            }
        } else if (collision.gameObject.tag == "Enemy") {
            if (isPiercing) return;
        }

        Destroy(gameObject);
    }

    private IEnumerator FadeOutAndDestroy()
    {
        yield return new WaitForSeconds(duration - fadeDuration);

        float fadeElapsed = 0f;
        Color originalColor = spriteRenderer.color;

        while (fadeElapsed < fadeDuration)
        {
            fadeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeElapsed / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
