using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class BossArm : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    
    public void MoveTo(Vector2 location) {
        rb.linearVelocity = (location - (Vector2)transform.position).normalized * 2f;

    }

}