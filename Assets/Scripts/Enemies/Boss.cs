using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class Boss : MonoBehaviour
{
    [SerializeField] private int bossState;
    [SerializeField] private int bossPhase;
    [SerializeField] GameObject projectile;
    [SerializeField] List<GameObject> fists = new List<GameObject>();
    private List<Vector2> originalFistOffset = new List<Vector2>();

    private float maxRadius = 4f;
    private Rigidbody2D rb;
    private Vector2 originPosition;
    private float elapsedTime;

    void Start(){
        bossState = 0;
        bossPhase = 0;
        StartCoroutine(StateLoop());

        // Store the original positions of each fist
        foreach (GameObject obj in fists) {
            if (obj != null) {
                originalFistOffset.Add(obj.transform.position - transform.position);
                obj.GetComponent<Collider2D>().enabled = false;
            }
        }
        
        originPosition = (Vector2)transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        elapsedTime+= Time.deltaTime;
        if (elapsedTime <= 2f) return;
        Vector2 direction = (SirGluten.playerPosition - (Vector2)transform.position).normalized;
        Vector2 newPosition = (Vector2)transform.position + direction * 1f * Time.fixedDeltaTime;

        if (Vector2.Distance(originPosition, newPosition) <= maxRadius) {
            rb.MovePosition(newPosition);

            for (int i = 0; i < fists.Count; i++) {
                if (fists[i] != null) {
                    fists[i].GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + originalFistOffset[i]);
                }
            }
        }
    }

    IEnumerator StateLoop(){
        SwitchState(0);
        yield return new WaitForSeconds(5f);

        // Pick random state
        switch(bossPhase) 
        {
        case 0:
            SwitchState(Random.Range(1, 3));
            break;
        default:
            break;
        }

        //Act accordingly
        switch(bossState) 
        {
        case 1:
            Debug.Log("FIST");
            StartCoroutine(MechaPunch());
            break;
        case 2:
            StartCoroutine(SpiralFire());
            break;
        case 3:
            Debug.Log("SPAWN");
            break;
        case 4:
            Debug.Log("BEAM");
            break;
        default:
            break;
        }

        while (bossState != 0) yield return null;
        StartCoroutine(StateLoop());
    }

    private void SwitchState(int state) {
        bossState = state;
        Debug.Log($"CURRENT STATE: {state}");
    }

    
    IEnumerator PulseFire() {
        Debug.Log("Pulse Fire");
        yield return new WaitForSeconds(1f);

        int projectileCount = 72;
        int pulses = 5;
        float timeBetweenPulse = 1f;
        float gap = 30f;
        float angleStep = 360f / projectileCount;

        for (int j = 0; j < pulses; j++) {
            Vector2 directionToPlayer = (SirGluten.playerPosition - (Vector2)transform.position).normalized;
            float gapAngle = (Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg + 360) % 360;
            Debug.Log(gapAngle);

            // Define the lower and upper bounds of the gap
            float lowerBound = (gapAngle - (gap / 2) + 360) % 360;
            float upperBound = (gapAngle + (gap / 2) + 360) % 360;

            for (int i = 0; i < projectileCount; i++) {
                float spawnAngle = i * angleStep;

                Quaternion newRotation = Quaternion.Euler(0, 0, spawnAngle);
                Vector2 spawnPosition = transform.position;

                GameObject newProjectile = Instantiate(projectile, spawnPosition, newRotation);
                newProjectile.transform.parent = GameManager.EffectStore.transform;

                Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
                rb.angularVelocity = -360f * 2;
            }
            
            yield return new WaitForSeconds(timeBetweenPulse);
        }
    }




    IEnumerator SpiralFire() {
        int projectileCount = 50;
        Debug.Log("Spiral Fire");
        yield return new WaitForSeconds(1f);

        Vector2 directionToPlayer = (SirGluten.playerPosition - (Vector2)transform.position).normalized;
        float currentAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 60;

        for (int i = 0; i < projectileCount; i++) {
            currentAngle = (currentAngle + 21f) % 360;
            Quaternion newRotation = Quaternion.Euler(0, 0, currentAngle);

            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);

            GameObject newProjectile = Instantiate(projectile, spawnPosition, newRotation);
            newProjectile.transform.parent = GameManager.EffectStore.transform;

            SpriteRenderer newProjSprite = newProjectile.GetComponent<SpriteRenderer>();
            //newProjSprite.color = currentColor;

            Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
            rb.angularVelocity = -360f * 2;
            yield return new WaitForSeconds(0.1f);
        }
        bossState = 0;
    }

    IEnumerator MechaPunch()
    {
        Rigidbody2D rb = fists[Random.Range(0, fists.Count)].GetComponent<Rigidbody2D>();
        Collider2D collider = rb.GetComponent<Collider2D>();

        Vector2 ogPosition = rb.position;
        Vector2 direction = (SirGluten.playerPosition - (Vector2)transform.position).normalized;


        //rb.constraints = RigidbodyConstraints2D.None;

        float lockTime = 0f;
        while(lockTime <= 3f) {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            lockTime += Time.deltaTime;
        }

        
        
        if (collider != null)
            collider.enabled = false;

        rb.linearVelocity = direction * 12f;


        yield return new WaitForSeconds(0.1f);
        
        if (collider != null)
            collider.enabled = true;

        yield return new WaitForSeconds(3f);

        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);

        while (Vector2.Distance(rb.position, ogPosition) > 0.1f)
        {
            Vector2 returnDirection = (ogPosition - rb.position).normalized;
            rb.linearVelocity = returnDirection * 10f;
            yield return null;
        }

        rb.position = ogPosition;
        rb.linearVelocity = Vector2.zero;

        if (collider != null)
            collider.enabled = false;

        //rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        bossState = 0;
    }




}