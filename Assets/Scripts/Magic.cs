using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Magic : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    [SerializeField] private string flavor;
    [SerializeField] private List<AudioClip> swingSFX;

    private SirGluten sirGluten;
    private Item item;
    private GameObject player;
    private Rigidbody2D body;
    private AudioSource audioSource;
    private Vector2 screenSize = new Vector2(Screen.width/2, Screen.height/2);
    private Vector2 mousePosition;
    private Animator playerAnimator;
    private Vector2 attackDirection;
    private string animationDirection;

    public global::System.Int32 AttackDamage { get => attackDamage; set => attackDamage = value; }
    public global::System.String AnimationDirection { get => animationDirection; set => animationDirection = value; }
    public global::System.String Flavor { get => flavor; set => flavor = value; }

    void Start() {
        player = GameObject.Find("SirGluten").gameObject;
        item = gameObject.GetComponent<Item>();
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
        if (Input.GetMouseButtonDown(0) && sirGluten.MainSlot != null && sirGluten.MainSlot.gameObject == gameObject && !sirGluten.IsAttacking) {
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

        CastMagic();
        yield return new WaitForSeconds(0.6f);

        sirGluten.MainSlot.transform.GetChild(1).gameObject.transform.position = body.position;
        sirGluten.WeaponAnimationFrame = 0;

        sirGluten.MainSlot.transform.GetChild(1).gameObject.SetActive(false);
        sirGluten.IsAnimationLocked = false;
        sirGluten.IsAttacking = false;
    }
    
    void CastMagic(){
        Debug.Log("hi");
    }
}
