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
    private Weapon weapon;
    private string weaponType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.Find("SirGluten").gameObject;
        sirGluten = player.GetComponent<SirGluten>();
        weapon = transform.parent.GetComponent<Weapon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int index = sirGluten.WeaponAnimationFrame;
        if (index == 0) {
            spriteRenderer.sprite = null;
        } else {
            if (weapon.AnimationDirection == "LR") {
                if (leftRightAttack.Count < index) {
                    index = leftRightAttack.Count;
                } 
                spriteRenderer.sprite = leftRightAttack[index-1];
            } else if (weapon.AnimationDirection == "F") {
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
