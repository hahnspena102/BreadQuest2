using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Ranged : MonoBehaviour
{
    private SirGluten sirGluten;
    private Weapon weapon;
    private GameObject player;
    private Rigidbody2D body;
    private AudioSource audioSource;
    private Vector2 screenSize = new Vector2(Screen.width/2, Screen.height/2);
    private Vector2 mousePosition;
    private Animator playerAnimator;
    private Vector2 attackDirection;  

    [SerializeField] private GameObject PlayerProjectile;  
    [SerializeField] private string projectileType = "singleshot";
    [SerializeField] private float angleRange = 0f;
    [SerializeField] private int numProj = 1;
    private bool mouseDown;
    private int drawingPhase = 0;
    private Coroutine rangedCoroutine = null;


    void Start() {
        player = GameObject.Find("SirGluten").gameObject;
        weapon = gameObject.GetComponent<Weapon>();
        playerAnimator = player.GetComponent<Animator>();
        body = player.GetComponent<Rigidbody2D>();
        sirGluten = player.GetComponent<SirGluten>();
        audioSource = player.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sirGluten.MainSlot != null && sirGluten.MainSlot.gameObject == gameObject) {
            if (sirGluten.IsAttacking) RangedDirection();
            if (sirGluten.IsAttacking) {
                sirGluten.MainSlot.transform.GetChild(1).gameObject.transform.position = body.position;
            }
            if (Input.GetMouseButtonDown(0) && sirGluten.MainSlot != null 
            && sirGluten.MainSlot.gameObject == gameObject && !sirGluten.IsAttacking 
            && !sirGluten.IsNavigatingUI) {
                rangedCoroutine = StartCoroutine(RangedAttack());
            }
            
            mouseDown = Input.GetMouseButton(0);

            if (!mouseDown && drawingPhase == 1 && rangedCoroutine != null) {
                playerAnimator.SetBool("rangedAttack",false);
                StopCoroutine(rangedCoroutine);
                EndRange();
            }

            if (drawingPhase >= 1) {
                sirGluten.Speed = 1f;
                
            } else {
                sirGluten.Speed = sirGluten.BaseSpeed;
            }
        }
        
    }

    private void RangedDirection() {
        mousePosition = (Vector2)Input.mousePosition - screenSize;
        
        Vector2 direction = (mousePosition - (Vector2)player.transform.position).normalized;
        
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 8;

        weapon.transform.rotation = Quaternion.Euler(0, 0, angle);

        
        if (sirGluten.MainSlot != null && sirGluten.MainSlot.gameObject == gameObject) {
            attackDirection = new Vector2(mousePosition.x, mousePosition.y).normalized;
        }

        if (mousePosition.x < 0) {
            sirGluten.transform.localScale = new Vector3(-1f, 1f, 1f);
            transform.localScale = new Vector3(-1f, -1f, 1f);
        } else {
            sirGluten.transform.localScale = new Vector3(1f, 1f, 1f);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

    }

    IEnumerator RangedAttack() {
          if (weapon.AttackSFX.Count > 0) {
            audioSource.clip = weapon.AttackSFX[Random.Range(0, weapon.AttackSFX.Count)];
            audioSource.pitch = (Random.Range(1.00f - 0.10f, 1.00f + 0.10f));
            audioSource.Play();
        }



        sirGluten.IsAnimationLocked = true;
        sirGluten.IsAttacking = true;

        playerAnimator.SetFloat("horizontal",Mathf.Abs(attackDirection.x));
        playerAnimator.SetFloat("vertical",attackDirection.y);
        playerAnimator.SetBool("rangedAttack",true);

        weapon.AnimationDirection = "LR";
        
        sirGluten.MainSlot.transform.GetChild(1).gameObject.SetActive(true);

        drawingPhase = 1;
        for (int i = 0; i < 3; i++) {
            sirGluten.WeaponAnimationFrame = i + 1;
            yield return new WaitForSeconds(0.3f);
        }

        drawingPhase = 2;
        while (mouseDown) {
            yield return null;
        }
        Shoot(projectileType);
        drawingPhase = 0;
        playerAnimator.SetBool("rangedAttack",false);
        
        for (int i = 3; i < 8; i++) {
            sirGluten.WeaponAnimationFrame = i + 1;
            yield return new WaitForSeconds(0.1f);
        }


        
    
        EndRange();
               
        yield return new WaitForSeconds(weapon.AttackCooldown);
    }

    void EndRange() {
        
        drawingPhase = 0;
        sirGluten.MainSlot.transform.GetChild(1).gameObject.transform.position = body.position;
        sirGluten.WeaponAnimationFrame = 0;

        sirGluten.MainSlot.transform.GetChild(1).gameObject.SetActive(false);
        sirGluten.IsAnimationLocked = false;
        sirGluten.IsAttacking = false;
    }
    
    void Shoot(string type){
        if (type == "multishot") {
            if (numProj == 1) {
            CreateProj(0);
            return;
            }

            float angleGap = angleRange / (numProj - 1);
            float startAngle = -angleRange / 2;

            for (int i = 0; i < numProj; i++) {
                float curAngle = startAngle + (i * angleGap);
                CreateProj(curAngle);
            }
            
        } else {
            CreateProj(0);
        }
    }

    GameObject CreateProj(float angleOffset) {
        Vector2 spawnPosition = new Vector2(body.position.x, body.position.y);
        Vector2 toMouse = ((Vector2)mousePosition - SirGluten.playerPosition).normalized;

        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, toMouse) * Quaternion.Euler(0, 0, angleOffset);

        GameObject newProjectile = Instantiate(PlayerProjectile, spawnPosition, rotation);
        newProjectile.transform.parent = GameManager.EffectStore.transform;

        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();

        Vector2 attackDirectionRotated = Quaternion.Euler(0, 0, angleOffset) * toMouse;
        if (rb.bodyType != RigidbodyType2D.Static) rb.linearVelocity = attackDirectionRotated; 

        PlayerProj mp = newProjectile.GetComponent<PlayerProj>();
        mp.AttackDamage = weapon.AttackDamage;
        mp.Flavor = weapon.Flavor;

        return newProjectile;
    }
}
