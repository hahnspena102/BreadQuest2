using UnityEngine;
using System.Collections;

public class Marshmoblin : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] GameObject spear;
    [SerializeField] private float attackOffset = 0f;
    [SerializeField] private float cooldown = 2f;
    
    void Start()
    {
    
        body = GetComponent<Rigidbody2D>();

        StartCoroutine(StartAttack());

    }

    IEnumerator StartAttack(){
        yield return new WaitForSeconds(attackOffset);
        StartCoroutine(Attack());
    }

    IEnumerator Attack(){
        /*while (Vector2.Distance(SirGluten.playerPosition.position, player.position) > 16f) {
            yield return null;
        } 
        */
        
        yield return new WaitForSeconds(0.6f);
        float upwardOffset = 0.2f;
        Vector2 spawnPosition = new Vector2(body.position.x - 0.58f, body.position.y - 0.15f);
        Vector2 directionToPlayer = (new Vector2(SirGluten.playerPosition.x, SirGluten.playerPosition.y) - (Vector2)transform.position).normalized;
        Vector2 adjustedDirection = new Vector2(directionToPlayer.x, directionToPlayer.y + upwardOffset).normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, adjustedDirection);
        GameObject newSpear = Instantiate(spear, spawnPosition, rotation);
        newSpear.transform.parent = transform;
        Rigidbody2D spearBody= newSpear.GetComponent<Rigidbody2D>();
        if (spearBody != null) {
            spearBody.linearVelocity = newSpear.transform.up * 8f; 
        }
        yield return new WaitForSeconds(cooldown);
        StartCoroutine(Attack());
    }
}
