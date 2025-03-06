using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Marshmoblin : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] GameObject spear;
    [SerializeField] private float attackOffset = 0f;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private List<AudioClip> attackSFX;
    private AudioSource audioSource;
    
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(StartAttack());

    }

    IEnumerator StartAttack(){
        yield return new WaitForSeconds(attackOffset);
        StartCoroutine(Attack());
    }

    IEnumerator Attack(){
        yield return new WaitForSeconds(0.6f);
        if (attackSFX.Count > 0) {
            audioSource.clip = attackSFX[Random.Range(0, attackSFX.Count)];
            audioSource.Play();
        }
        Vector2 spawnPosition = new Vector2(body.position.x, body.position.y);
        Vector2 directionToPlayer = (new Vector2(SirGluten.playerPosition.x, SirGluten.playerPosition.y) - (Vector2)transform.position).normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, directionToPlayer);
        GameObject newSpear = Instantiate(spear, spawnPosition, rotation);
        newSpear.transform.parent = GameManager.EffectStore.transform;
        yield return new WaitForSeconds(cooldown);
        StartCoroutine(Attack());
    }
}
