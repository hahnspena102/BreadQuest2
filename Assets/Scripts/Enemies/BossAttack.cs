using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField]private float timeTilCollision = 0.05f;

    private float elapsedTime = 0;
    private Collider hitbox;

    public global::System.Int32 Damage { get => damage; set => damage = value; }
    void Start(){
        hitbox = GetComponent<Collider>();

        if (hitbox != null)
        {
            hitbox.enabled = false;
        }
    }

    void Update(){
        elapsedTime += Time.deltaTime;

        if (hitbox != null && elapsedTime >= timeTilCollision)
        {
            hitbox.enabled = true;
        }
    }

    
}