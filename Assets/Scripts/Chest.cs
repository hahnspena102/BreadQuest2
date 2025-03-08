using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    private GameObject hoverText;
    private bool isOpenable, isOpened;
    private GameManager gameManager;
    private Animator animator;
    private List<List<GameObject>> itemsByTier = new List<List<GameObject>>();

    [SerializeField]private int tier = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hoverText = transform.GetChild(0).gameObject;
        gameManager = GameObject.Find("Game").gameObject.GetComponent<GameManager>();
        animator = GetComponent<Animator>();

        itemsByTier.Add(gameManager.ItemsTier1);
        itemsByTier.Add(gameManager.ItemsTier2);
        itemsByTier.Add(gameManager.ItemsTier3);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenable && Input.GetKeyDown(KeyCode.E) && !isOpened) {
            StartCoroutine(OpenChest());
            //CreateItem(tier);
        }
    }

    IEnumerator OpenChest(){
        animator.SetTrigger("unlock");
        isOpened = true;
        yield return new WaitForSeconds(1f);
        CreateItem();
        Destroy(gameObject);
    }

    void CreateItem() {
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
            Weapon weapon = gameObj.GetComponent<Weapon>();
            randomizerMap[curNum] = gameObj;
            curNum += GameManager.RarityMap[weapon.Rarity];
            //Debug.Log(item.Rarity);
        }

        int randomNumber = Random.Range(0, curNum);
        while (!randomizerMap.ContainsKey(randomNumber)) {
            randomNumber--;
        }

        GameObject newWeapon = Instantiate(randomizerMap[randomNumber], transform.position, Quaternion.identity);
        newWeapon.transform.SetParent(GameManager.ItemStore.transform);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Player" && !isOpened) {
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
