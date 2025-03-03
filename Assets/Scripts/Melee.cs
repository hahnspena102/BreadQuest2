using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Melee : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    private SirGluten sirGluten;
    private GameObject player;
    private Rigidbody2D body;
    private Vector2 screenSize = new Vector2(Screen.width/2, Screen.height/2);
    private Vector2 mousePosition;
    private Animator playerAnimator;
    private Vector2 attackPosition;

    public global::System.Int32 AttackDamage { get => attackDamage; set => attackDamage = value; }

    void Start() {
        player = GameObject.Find("SirGluten").gameObject;
        playerAnimator = player.GetComponent<Animator>();
        body = player.GetComponent<Rigidbody2D>();
        sirGluten = player.GetComponent<SirGluten>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sirGluten.IsAttacking) MeleeDirection();
        if (Input.GetMouseButtonDown(0) && sirGluten.MainSlot != null && !sirGluten.IsAttacking) {
            MeleeDirection();
            StartCoroutine(MeleeAttack());
        }
    }

    private void MeleeDirection() {
        if (!sirGluten.IsAttacking) {
            mousePosition = (Vector2)Input.mousePosition - screenSize;
        }
        if (sirGluten.MainSlot != null) {
            if ((mousePosition.y != 0 || mousePosition.x != 0)) {
                Rigidbody2D attackRB = sirGluten.MainSlot.transform.GetChild(1).gameObject.GetComponent<Rigidbody2D>();
                if (Mathf.Abs(mousePosition.x) > Mathf.Abs(mousePosition.y)) {
                    attackRB.position = body.position + new Vector2(mousePosition.x, 0).normalized;
                    attackPosition = new Vector2(mousePosition.x, 0).normalized;
                } else {
                    attackRB.position = body.position + new Vector2(0, mousePosition.y).normalized;
                    attackPosition = new Vector2(0, mousePosition.y).normalized;
                }
                
            }
        }
    }

    IEnumerator MeleeAttack() {
        // Animation Start

        sirGluten.IsAnimationLocked = true;
        sirGluten.IsAttacking = true;
        playerAnimator.SetTrigger("meleeAttack");
        

        sirGluten.MainSlot.transform.GetChild(2).gameObject.SetActive(true);
        Animator itemAnimator = sirGluten.MainSlot.transform.GetChild(2).gameObject.GetComponentInChildren<Animator>();

        playerAnimator.SetFloat("vertical",attackPosition.y);
        itemAnimator.SetFloat("vertical",attackPosition.y);
        playerAnimator.SetFloat("horizontal",Mathf.Abs(attackPosition.x));
        itemAnimator.SetFloat("horizontal",Mathf.Abs(attackPosition.x));

        if (attackPosition.x < 0) {
            Vector2 rotator = new Vector3(transform.rotation.x, 180f);
            sirGluten.transform.rotation = Quaternion.Euler(rotator);
            transform.rotation = Quaternion.Euler(rotator);
        } else {
            Vector2 rotator = new Vector3(transform.rotation.x, 0f);
            sirGluten.transform.rotation = Quaternion.Euler(rotator);
            transform.rotation = Quaternion.Euler(rotator);
        }
        

        // Attack
        yield return new WaitForSeconds(0.2f);

        sirGluten.MainSlot.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        // Animation End
        sirGluten.MainSlot.transform.GetChild(2).gameObject.transform.position = body.position;

        
        sirGluten.MainSlot.transform.GetChild(1).gameObject.SetActive(false);
        sirGluten.MainSlot.transform.GetChild(2).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        sirGluten.IsAnimationLocked = false;
        sirGluten.IsAttacking = false;
    }
}
