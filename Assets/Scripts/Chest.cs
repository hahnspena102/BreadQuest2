using UnityEngine;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    private GameObject hoverText;
    private bool isOpenable;
    private GameManager gameManager;
    private List<List<GameObject>> itemsByTier = new List<List<GameObject>>();

    [SerializeField]private int tier = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hoverText = transform.GetChild(0).gameObject;
        gameManager = GameObject.Find("Game").gameObject.GetComponent<GameManager>();

        itemsByTier.Add(gameManager.ItemsTier1);
        itemsByTier.Add(gameManager.ItemsTier2);
        itemsByTier.Add(gameManager.ItemsTier3);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenable && Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("OPEN");
            CreateItem(tier);
        }
    }

    void CreateItem(int tier) {
        List<GameObject> combinedList = new List<GameObject>();
        combinedList.AddRange(itemsByTier[0]);
        if (tier >= 2) {
            combinedList.AddRange(itemsByTier[1]);
        }
        if (tier >= 3) {
            combinedList.AddRange(itemsByTier[2]);
        }
        //Debug.Log(combinedList.Count);

        int curNum = 0;
        Dictionary<int, GameObject> randomizerMap = new Dictionary<int, GameObject>();
        foreach (GameObject gameObj in combinedList) {
            Item item = gameObj.GetComponent<Item>();
            randomizerMap[curNum] = gameObj;
            curNum += GameManager.RarityMap[item.Rarity];
            //Debug.Log(item.Rarity);
        }

        int randomNumber = Random.Range(0, curNum);
        while (!randomizerMap.ContainsKey(randomNumber)) {
            randomNumber--;
        }

        GameObject newItem = Instantiate(randomizerMap[randomNumber], transform.position, Quaternion.identity);
        newItem.transform.parent = GameManager.ItemStore.transform;
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Player") {
            hoverText.SetActive(true);
            isOpenable = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.tag == "Player") {
            hoverText.SetActive(false);
            isOpenable = false;
        }
    }
}
