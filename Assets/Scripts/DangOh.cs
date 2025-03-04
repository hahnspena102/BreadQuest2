using UnityEngine;
using System.Collections;
using System;

public class DangOh : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;

    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackCooldown = 30f; 
    [SerializeField] private float spreadAngle = 15f; // Spread angle for 3 shots
    [SerializeField] private float detectionRadius = 0.1f; // Player detection range
    [SerializeField] private float animationLength = 3f; // Adjust this based on your attack animation length

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Get Animator component
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            float playerDistance = Vector2.Distance(SirGluten.playerPosition, transform.position);

            if (playerDistance <= detectionRadius) // Only attack if player is close
            {
                yield return StartCoroutine(Attack());
            }
            
            yield return new WaitForSeconds(attackCooldown); // Wait before next attack
        }
    }

    private IEnumerator Attack()
    {
        animator.SetTrigger("AttackTrigger"); // Play attack animation
        yield return new WaitForSeconds(animationLength); // Wait for animation to finish
        FireProjectiles(); // Fire projectiles only after animation ends
    }

    private void FireProjectiles()
    {
        Vector2 spawnPosition = body.position;
        Vector2 directionToPlayer = (SirGluten.playerPosition - (Vector2)transform.position).normalized;
        Quaternion baseRotation = Quaternion.FromToRotation(Vector2.up, directionToPlayer);

        float[] angles = { -spreadAngle, 0f, spreadAngle }; // Spread angles for 3 shots

        Color[] projectileColors =
        {
            new Color(1.0f, 0.95f, 0.8f),  
            new Color(0.6f, 1.0f, 0.6f),  // Light Bright Green
            new Color(1.0f, 0.82f, 0.86f) // Pastel Pink
        };

        for (int i = 0; i < angles.Length; i++)
        {
            Quaternion rotation = baseRotation * Quaternion.Euler(0, 0, angles[i]);
            GameObject newProjectile = Instantiate(projectile, spawnPosition, rotation);
            newProjectile.transform.parent = transform;

            if (newProjectile.TryGetComponent<SpriteRenderer>(out SpriteRenderer projSprite))
            {
                projSprite.color = projectileColors[i]; // Assign color to each projectile
            }
        }
    }
}
