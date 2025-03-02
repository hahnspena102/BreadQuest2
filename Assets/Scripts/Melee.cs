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

    public global::System.Int32 AttackDamage { get => attackDamage; set => attackDamage = value; }

    void Start() {
        player = gameObject.transform.parent.gameObject;
        sirGluten = GameObject.Find("SirGluten").gameObject.GetComponent<SirGluten>();
        body = sirGluten.GetComponent<Rigidbody2D>();
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
                } else {
                    attackRB.position = body.position + new Vector2(0, mousePosition.y).normalized;
                }
                
            }
        }
    }

    IEnumerator MeleeAttack() {
        sirGluten.IsAttacking = true;
        sirGluten.MainSlot.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        sirGluten.IsAttacking = false;
        sirGluten.MainSlot.transform.GetChild(1).gameObject.SetActive(false);
    }
}
