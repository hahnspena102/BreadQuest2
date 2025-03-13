using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Statue : MonoBehaviour
{
    private SirGluten sirGluten;
    [SerializeField]private GameObject hoverText;
    private bool isOpenable;
    private GameManager gameManager;
    private UIManager uiManager;
    private List<Passive> passivesSold = new List<Passive>();

    [SerializeField]private int tier = 1;
    private int hPotStock = 3, gPotStock = 3, potionCost = 25;

    public global::System.Int32 HPotStock { get => hPotStock; set => hPotStock = value; }
    public global::System.Int32 GPotStock { get => gPotStock; set => gPotStock = value; }
    public global::System.Int32 PotionCost { get => potionCost; set => potionCost = value; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sirGluten = GameObject.Find("SirGluten").gameObject.GetComponent<SirGluten>();
        gameManager = GameObject.Find("Game").gameObject.GetComponent<GameManager>();
        uiManager = GameObject.Find("UI").gameObject.GetComponent<UIManager>();


        List<Passive> combinedList = new List<Passive>();
        combinedList.AddRange(gameManager.PassivesTier1);
        if (tier >= 2) {
            combinedList.AddRange(gameManager.PassivesTier2);
        }
        if (tier >= 3) {
            combinedList.AddRange(gameManager.PassivesTier3);
        }

        List<int> randomIndices = GetUniqueRandomNumbers(combinedList.Count, Mathf.Min(combinedList.Count, 3));
        foreach (int index in randomIndices)
        {
            passivesSold.Add(combinedList[index]);
        }
    }

    List<int> GetUniqueRandomNumbers(int listLength, int numOfNumbers)
    {
        HashSet<int> uniqueIndices = new HashSet<int>();
        
        while (uniqueIndices.Count < numOfNumbers)
        {
            int randomIndex = Random.Range(0, listLength);
            uniqueIndices.Add(randomIndex);
        }

        return new List<int>(uniqueIndices);
    }


    void Update() {
        if (isOpenable && Input.GetKeyDown(KeyCode.E)) {
            uiManager.ActivateShop(gameObject.GetComponent<Statue>(), passivesSold);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            hoverText.SetActive(true);
            isOpenable = true;

        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            hoverText.SetActive(false);
            uiManager.DeactivateShop();
            isOpenable = false;
        }
    }
}
