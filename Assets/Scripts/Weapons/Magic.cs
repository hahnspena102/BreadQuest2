using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Magic : MonoBehaviour
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

    [SerializeField] private GameObject magicProjectile;  
    [SerializeField] private int glucoseCost;
    [SerializeField] private string projectileType = "singleshot";
    [SerializeField] private float angleRange = 0f;
    [SerializeField] private int numProj = 1;

    public global::System.Int32 GlucoseCost { get => glucoseCost; set => glucoseCost = value; }

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
        if (sirGluten.IsAttacking) {
            MagicDirection();
            sirGluten.MainSlot.transform.GetChild(1).gameObject.transform.position = body.position;
        }
        if (Input.GetMouseButtonDown(0) && sirGluten.MainSlot != null 
        && sirGluten.MainSlot.gameObject == gameObject && !sirGluten.IsAttacking 
        && sirGluten.Glucose >= glucoseCost && !sirGluten.IsNavigatingUI) {
            MagicDirection();
            StartCoroutine(MagicAttack());
        }
    }

    private void MagicDirection() {
        if (!sirGluten.IsAttacking) {
            mousePosition = (Vector2)Input.mousePosition - screenSize;
        }
        if (sirGluten.MainSlot != null && sirGluten.MainSlot.gameObject == gameObject) {
            attackDirection = new Vector2(mousePosition.x, mousePosition.y).normalized;
        }
    }

    IEnumerator MagicAttack() {
        sirGluten.Glucose -= glucoseCost;

        sirGluten.IsAnimationLocked = true;
        sirGluten.IsAttacking = true;

        playerAnimator.SetFloat("horizontal",Mathf.Abs(attackDirection.x));
        playerAnimator.SetFloat("vertical",attackDirection.y);
        playerAnimator.SetTrigger("magicAttack");
        

        if (attackDirection.x < 0) {
            Vector2 rotator = new Vector3(transform.rotation.x, 180f);
            sirGluten.transform.rotation = Quaternion.Euler(rotator);
            transform.rotation = Quaternion.Euler(rotator);
        } else {
            Vector2 rotator = new Vector3(transform.rotation.x, 0f);
            sirGluten.transform.rotation = Quaternion.Euler(rotator);
            transform.rotation = Quaternion.Euler(rotator);
        }

        
        sirGluten.MainSlot.transform.GetChild(1).gameObject.SetActive(true);

        CastMagic(projectileType);
        yield return new WaitForSeconds(0.6f);

        sirGluten.MainSlot.transform.GetChild(1).gameObject.transform.position = body.position;
        sirGluten.WeaponAnimationFrame = 0;

        sirGluten.MainSlot.transform.GetChild(1).gameObject.SetActive(false);
        sirGluten.IsAnimationLocked = false;
        sirGluten.IsAttacking = false;
    }
    
    void CastMagic(string type){
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

        GameObject newProjectile = Instantiate(magicProjectile, spawnPosition, rotation);
        newProjectile.transform.parent = GameManager.EffectStore.transform;

        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();

        Vector2 attackDirectionRotated = Quaternion.Euler(0, 0, angleOffset) * toMouse;
        if (rb.bodyType != RigidbodyType2D.Static) rb.linearVelocity = attackDirectionRotated; 

        MagicProj mp = newProjectile.GetComponent<MagicProj>();
        mp.AttackDamage = weapon.AttackDamage;
        mp.Flavor = weapon.Flavor;

        return newProjectile;
    }
}
