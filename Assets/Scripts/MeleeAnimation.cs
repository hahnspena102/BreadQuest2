using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MeleeAnimation : MonoBehaviour
{
    [SerializeField]private List<Sprite> forwardSwing = new List<Sprite>();
    [SerializeField]private List<Sprite> backwardSwing = new List<Sprite>();
    [SerializeField]private List<Sprite> leftRightSwing = new List<Sprite>();
    private SirGluten sirGluten;
    private SpriteRenderer spriteRenderer;
    private Melee melee;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.Find("SirGluten").gameObject;
        sirGluten = player.GetComponent<SirGluten>();
        melee = transform.parent.GetComponent<Melee>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int index = sirGluten.WeaponAnimationFrame;
        if (index == 0) {
            spriteRenderer.sprite = null;
        } else {
            if (melee.AnimationDirection == "LR") {
                spriteRenderer.sprite = leftRightSwing[index-1];
            } else if (melee.AnimationDirection == "F") {
                spriteRenderer.sprite = forwardSwing[index-1];
            } else {
                spriteRenderer.sprite = backwardSwing[index-1];
            }
        }
    
        
    }
}
