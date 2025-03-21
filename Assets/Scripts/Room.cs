using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Room : MonoBehaviour
{
    [SerializeField] private Transform wavesParent;
    [SerializeField] private GameObject barriers;
    [SerializeField] private GameObject chest;
    [SerializeField] private GameObject spawnParticles;

    private List<Transform> waveList = new List<Transform>();
    private List<GameObject> activeEnemies = new List<GameObject>();

    private bool waveActive = false;
    private int waveIndex = 0;

    public Transform WavesParent { get => wavesParent; set => wavesParent = value; }
    public GameObject Barriers { get => barriers; set => barriers = value; }
    public GameObject Chest { get => chest; set => chest = value; }
    public GameObject SpawnParticles { get => spawnParticles; set => spawnParticles = value; }

    private void Start()
    {
        if (barriers != null) barriers.SetActive(false);
        if (chest != null) chest.SetActive(false);
        foreach (Transform wave in wavesParent) {
            waveList.Add(wave);
        }
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (!waveActive && collision.gameObject.CompareTag("Player")) {
            waveActive = true;
            ActiveBarrier();
            StartCoroutine(StartWaves());
        }
    }


    private void ActiveBarrier(){
        if (barriers != null) barriers.SetActive(true);
    }

    private IEnumerator StartWaves()
    {
        while (waveIndex < waveList.Count) {
            yield return new WaitForSeconds(1f);
            Transform wave = waveList[waveIndex];
            foreach (Transform enemy in wave)
            {
                GameObject activeEnemy = Instantiate(enemy.gameObject, enemy.position, Quaternion.identity);
                GameManager.PlayParticle(spawnParticles, enemy.position);
                activeEnemies.Add(activeEnemy);
            }
            yield return new WaitUntil(() => activeEnemies.Count == 0);
            waveIndex++;
        }
        waveActive = false;
        if (chest != null) {
            chest.SetActive(true);
            chest.transform.SetParent(GameManager.InteractableStore.transform);
        }
        Destroy(barriers);
        Destroy(transform.parent.gameObject, 5f);
    }

    private void Update()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
            }
        }
    }

}
