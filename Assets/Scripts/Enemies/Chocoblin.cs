using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Chocoblin : MonoBehaviour
{
    [SerializeField] private int boundaryHeight, boundaryWidth;
    [SerializeField] private Tilemap tilemap;
    private List<Vector2> allTilePositions;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private GameObject projectile;
    private GameObject player;
    private Rigidbody2D rb;
    private Animator animator;
    private float attackCooldown = 1.4f, timeBeforeCast = 1.4f;

    void Start()
    {

        StartCoroutine(ChocoblinSequence());

        player = GameObject.Find("SirGluten");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        tilemap = GameObject.Find("Floor").GetComponent<Tilemap>();
        allTilePositions = GetAllTilePositions();

        
    }

    void Update(){
        if (rb != null) rb.linearVelocity = Vector2.zero;

        if (player.transform.position.x > transform.position.x) {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    List<Vector2> GetAllTilePositions()
    {
        if (tilemap == null) return null;
        List<Vector2> tilePositions = new List<Vector2>();

        BoundsInt bounds = tilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                if (Mathf.Abs(pos.x - transform.position.x) > boundaryWidth) continue;
                if (Mathf.Abs(pos.y - transform.position.y) > boundaryHeight) continue;
                tilePositions.Add(new Vector2(pos.x,pos.y));
            }
        }

        return tilePositions;
    }

    IEnumerator ChocoblinSequence()
    {
        while (allTilePositions == null || player == null) yield return null;

        Vector2 newPos = allTilePositions[Random.Range(0,allTilePositions.Count)];

        while (Vector2.Distance((Vector2)player.transform.position, newPos) < 3f || inWall(newPos))
        {
            newPos = allTilePositions[Random.Range(0,allTilePositions.Count)];
        }

        TeleportTo(newPos);
        yield return new WaitForSeconds(timeBeforeCast/2f);
        animator.SetBool("isFloating",true);
        yield return new WaitForSeconds(timeBeforeCast/2f);

        CastMagic();
        yield return new WaitForSeconds(attackCooldown/2f);
        animator.SetBool("isFloating",false);
        yield return new WaitForSeconds(attackCooldown/2f);
        StartCoroutine(ChocoblinSequence());
    }

bool inWall(Vector2 pos)
{
    float radius = 0.5f;

    Collider2D hit = Physics2D.OverlapCircle(pos, radius, wallLayer);

    return hit != null;
}

    void TeleportTo(Vector2 pos)
    {
        rb.position = pos;
    }

    void CastMagic()
    {
        Vector2 spawnPosition = new Vector2(rb.position.x, rb.position.y);
        Vector2 directionToPlayer = (new Vector2(SirGluten.playerPosition.x, SirGluten.playerPosition.y) - (Vector2)transform.position).normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, directionToPlayer);

        GameObject newProjectile = Instantiate(projectile, spawnPosition, rotation);
        newProjectile.transform.parent = GameManager.EffectStore.transform;
    }
}
