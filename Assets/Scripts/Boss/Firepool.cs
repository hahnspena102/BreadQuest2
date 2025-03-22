using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Firepool : MonoBehaviour
{
    private Animator animator;
    private Collider2D fpCollider;
    void Start() {
        StartCoroutine(SpawnCoroutine());
        animator = GetComponent<Animator>();
        fpCollider =GetComponent<Collider2D>();

    }

    IEnumerator SpawnCoroutine(){
        yield return new WaitForSeconds(5f);
        fpCollider.enabled = true;

        yield return new WaitForSeconds(10f);
        animator.SetTrigger("death");
        Destroy(gameObject,0.5f);
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
         
            SirGluten sirGluten = other.gameObject.GetComponent<SirGluten>();
            if (sirGluten == null) return;
            
            sirGluten.BurnTime = 2f;
        }
    }
}