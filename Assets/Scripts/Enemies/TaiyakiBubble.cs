using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TaiyakiBubble : MonoBehaviour
{
private Rigidbody2D rb;
private SirGluten sirGluten;
[SerializeField]float speed= 2f,duration = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
        Destroy(gameObject, duration);
    }

    void Update(){
        if (rb == null) return;
        Vector2 direction = ((Vector2)sirGluten.transform.position - rb.position).normalized;
        rb.linearVelocity = direction * speed;
    }
}