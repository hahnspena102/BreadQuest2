using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private bool isActive = false;
    private Rigidbody2D rb;
    private Animator animator;

    public bool IsActive { get => isActive; set => isActive = value; }
    public Rigidbody2D Rb { get => rb; set => rb = value; }
    public Animator Animator { get => animator; set => animator = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(SpikeSequence());
    }

    void Update()
    {
        animator.SetBool("isActive", isActive);   
        Debug.Log($"isActive:" + isActive);
    }

    IEnumerator SpikeSequence(){
        isActive = true;
        yield return new WaitForSeconds(1f);
        isActive = false;
        yield return new WaitForSeconds(2f);
        StartCoroutine(SpikeSequence());
    }



}
