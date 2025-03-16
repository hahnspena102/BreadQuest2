using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class Boss : MonoBehaviour
{
    [SerializeField]private int bossState;
    [SerializeField]private int bossPhase;
    [SerializeField] GameObject projectile;
    [SerializeField] List<GameObject> fists = new List<GameObject>();

    void Start(){
        bossState = 0;
        bossPhase = 0;
        StartCoroutine(StateLoop());
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
            fists[Random.Range(0,fists.Count)].GetComponent<BossArm>().MoveTo(SirGluten.playerPosition);
            bossState = 0;
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
    

}