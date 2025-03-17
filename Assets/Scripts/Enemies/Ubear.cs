using UnityEngine;

public class Ubear : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SirGluten sirGluten;
    [SerializeField] AudioSource slashSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null && rb != null) {
            animator.SetBool("isMoving",rb.linearVelocity.magnitude > 0.2f);
            animator.SetBool("isClose",Vector2.Distance(rb.position,(Vector2)sirGluten.transform.position) <= 2f);
        }
        
    }

    public void PlaySlash() {
        slashSFX.pitch = Random.Range(1.00f - 0.10f, 1.00f + 0.10f);
        slashSFX.Play();
    }
}
