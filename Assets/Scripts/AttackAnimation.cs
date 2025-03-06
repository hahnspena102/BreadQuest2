using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AttackAnimation : MonoBehaviour
{
    [SerializeField]private List<Sprite> forwardAttack = new List<Sprite>();
    [SerializeField]private List<Sprite> backwardAttack = new List<Sprite>();
    [SerializeField]private List<Sprite> leftRightAttack = new List<Sprite>();
    private SirGluten sirGluten;
    private SpriteRenderer spriteRenderer;
    private Melee melee;
    private Magic magic;
    private string weaponType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.Find("SirGluten").gameObject;
        sirGluten = player.GetComponent<SirGluten>();
        melee = transform.parent.GetComponent<Melee>();
        magic = transform.parent.GetComponent<Magic>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int index = sirGluten.WeaponAnimationFrame;
        if (index == 0) {
            spriteRenderer.sprite = null;
        } else {
            string direction;
            if (melee != null) {
                direction = melee.AnimationDirection;
            } else if (magic != null) {
                direction = magic.AnimationDirection;
            } else {
                direction = "";
            }
        
            if (direction == "LR") {
                if (leftRightAttack.Count < index) {
                    index = leftRightAttack.Count;
                } 
                spriteRenderer.sprite = leftRightAttack[index-1];
            } else if (direction == "F") {
                if (forwardAttack.Count < index) {
                    index = forwardAttack.Count;
                } 
                spriteRenderer.sprite = forwardAttack[index-1];
            } else {
                if (backwardAttack.Count < index) {
                    index = backwardAttack.Count;
                } 
                spriteRenderer.sprite = backwardAttack[index-1];
            }
        }
    
        
    }

    void Awake(){
        if (sirGluten != null) sirGluten.WeaponAnimationFrame = 0;
    }
}
