using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]private List<GameObject> enemies = new List<GameObject>();
    [SerializeField]private List<int> enemySwarmCounts = new List<int>();
    private Animator animator;

    void Start() {
        StartCoroutine(SpawnCoroutine());
        animator = GetComponent<Animator>();
    }

    IEnumerator SpawnCoroutine(){
        yield return new WaitForSeconds(1f);
        int randomIndex = Random.Range(0,enemies.Count);
        int swarmCount = 0;

        if (randomIndex >= enemySwarmCounts.Count) {
            swarmCount = 1;
        } else {
            swarmCount = enemySwarmCounts[randomIndex];
        }
        
        for (int i = 0; i < swarmCount; i++) {
            GameObject newEnemy = Instantiate(enemies[randomIndex],transform.position,Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
        

        yield return new WaitForSeconds(1f);
        animator.SetTrigger("death");
        Destroy(gameObject,0.5f);
    }
    
}