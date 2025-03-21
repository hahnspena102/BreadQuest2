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
    [SerializeField]private GameObject marker;
    private List<Vector2> originalFistOffset = new List<Vector2>();
    private List<bool> fistsAttacking = new List<bool>();

    private float maxRadius = 4f;
    private Rigidbody2D rb;
    private Vector2 originPosition;
    private float elapsedTime;
    private LayerMask wallLayer;

    [SerializeField]private AudioSource soundtrack;
    private Enemy enemy;

    private Vector2 arenaLowerLeft = new Vector2(15.5f,-4.5f);
    private Vector2 arenaUpperRight = new Vector2(41.5f, 10.5f);
    

    void Start(){
        wallLayer = LayerMask.GetMask("Walls");
        enemy = GetComponent<Enemy>();
        bossState = 0;
        bossPhase = 0;
        StartCoroutine(StateLoop());

        // Store the original positions of each fist
        foreach (GameObject obj in fists) {
            if (obj != null) {
                originalFistOffset.Add(obj.transform.localPosition);
                fistsAttacking.Add(false);
                obj.GetComponent<Collider2D>().enabled = false;
            }
        }
        
        originPosition = (Vector2)transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        elapsedTime+= Time.deltaTime;
        if (elapsedTime <= 2f) return;
        Vector2 direction = (SirGluten.playerPosition - (Vector2)transform.position + new Vector2(0,3f)).normalized;
        Vector2 newPosition = (Vector2)transform.position + direction * 1f * Time.fixedDeltaTime;

        if (Vector2.Distance(originPosition, newPosition) <= maxRadius) {
            rb.MovePosition(newPosition);

            for (int i = 0; i < fists.Count; i++) {
                if (fistsAttacking[i]) continue;
                Vector2 fistOffset = originalFistOffset[i];
                Rigidbody2D fistRB = fists[i].GetComponent<Rigidbody2D>();
                fistRB.position = fistOffset + rb.position;
            }
        }
    }

    void Update(){
        int curPhase = bossPhase;
        
        if (enemy.Health > enemy.MaxHealth * 2 / 3) {
            bossPhase = 1;
            if (curPhase != bossPhase) {
                Debug.Log("phase 1!");
            }
        } 
        else if (enemy.Health > enemy.MaxHealth / 3) {
            bossPhase = 2;
            if (curPhase != bossPhase) {
                Debug.Log("phase 2!");
            }
        } 
        else {
            bossPhase = 3;
            if (curPhase != bossPhase) {
                Debug.Log("phase 3!");
            }
        }
    }

    IEnumerator StateLoop(){
        SwitchState(0);
        yield return new WaitForSeconds(1f);

        // Pick random state
        switch(bossPhase) 
        {
        case 1:
            SwitchState(Random.Range(2, 3));
            break;
        case 2:
            SwitchState(Random.Range(2, 3));
            break;
        case 3:
            SwitchState(Random.Range(2, 3));
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
            Debug.Log("FIRE");
            StartCoroutine(PulseFire());
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
    }

    
    // PHASE 1 MECHANICS
    IEnumerator SpiralFire() {
        int projectileCount = 50;
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
        int randomNumber = Random.Range(0, fists.Count);
        Vector2 fistOffset = originalFistOffset[randomNumber];
        Rigidbody2D fistRB = fists[randomNumber].GetComponent<Rigidbody2D>();
        Collider2D collider = fistRB.GetComponent<Collider2D>();

        // ROTATE
        float lockTime = 0f;
        while(lockTime <= 3f) {
            Vector2 curDirection = SirGluten.playerPosition - fistRB.position;
            if (fistRB.transform.localScale.x < 0) {
                fistRB.rotation = -(Mathf.Atan2(curDirection.y, -curDirection.x) * Mathf.Rad2Deg);
            } else {
                fistRB.rotation = Mathf.Atan2(curDirection.y, curDirection.x) * Mathf.Rad2Deg;
            }
            
            lockTime += Time.deltaTime;
            yield return null;
        }

        // TO SIRGLUTEN'S LOCATION
        fistsAttacking[randomNumber] = true;
        Vector2 trackedPosition = SirGluten.playerPosition;
        fistRB.linearVelocity = (trackedPosition - fistRB.position).normalized * 8f;
        if (collider != null) collider.enabled = true;

        while (true) {
            RaycastHit2D hit = Physics2D.Raycast(fistRB.position, fistRB.linearVelocity.normalized, 0.1f, wallLayer);
            if (hit.collider != null) {
                break; 
            }

            yield return null;
        }

        

        // RETURN
        if (collider != null) collider.enabled = false;
        while (Vector2.Distance((fistOffset + rb.position),fistRB.position) > 0.2f) {
            fistRB.linearVelocity = ((fistOffset + rb.position) - fistRB.position).normalized * 3f;
            GameObject newmarker = Instantiate(marker,(fistOffset + rb.position),Quaternion.identity);
            Destroy(newmarker,2f);
            yield return null;
        }
        fistsAttacking[randomNumber] = false;
        fistRB.linearVelocity = Vector2.zero;
        fistRB.rotation = 0;

        // COOLDOWN
        yield return new WaitForSeconds(1f);
        bossState = 0;
    }

    // PHASE 2 MECHANICS
    IEnumerator PulseFire() {
        Debug.Log("pulse fire!");

        Vector2 directionToPlayer = (SirGluten.playerPosition - (Vector2)transform.position).normalized;
        float playerAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        //Debug.Log(playerAngle);

        int projectileCount = 128;
        int pulseCount = 3;
        float angleGap = 30f;

        for (int i = 0; i < pulseCount; i++) {
            float angleOffset = Random.Range(-60,60);
            for (int j = 0; j < projectileCount; j++) {
                float currentAngle = j * (360f / projectileCount); 

                Quaternion newRotation = Quaternion.Euler(0, 0, currentAngle);

                if (Mathf.Abs(Mathf.DeltaAngle(currentAngle, playerAngle - 90 + angleOffset)) < angleGap) {
                    Debug.Log(currentAngle);
                    continue;
                }

                Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);

                GameObject newProjectile = Instantiate(projectile, spawnPosition, newRotation);
                newProjectile.transform.parent = GameManager.EffectStore.transform;

                SpriteRenderer newProjSprite = newProjectile.GetComponent<SpriteRenderer>();

                Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
            rb.angularVelocity = -360f * 2;
            }
            yield return new WaitForSeconds(1.5f);
        }


        yield return new WaitForSeconds(3f);
        bossState = 0;
        
    }

}