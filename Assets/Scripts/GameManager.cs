using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameObject ProjectileStore;
    public static GameObject ItemStore;
    public static GameObject EffectStore;
    public static GameObject SoundPrefab;
    public GameObject inspectorSoundPrefab;
    public static Dictionary<string, Color> FlavorColorMap = new Dictionary<string, Color>()
    {
        { "flavorless", new Color(0.680506f, 0.5850837f,0.7169812f)},
        { "vanilla", new Color(1f, 0.9617949f, 0.7971698f)},
        { "cacao", new Color(0.509434f,0.362371f,0.3147918f) },
        { "fruity", new Color(1f, 0.673f, 0.8505448f)},
        { "grain", new Color(1f,0.8153504f,0.4481132f)},
        { "spicy", new Color(1f, 0.7417727f, 0.495283f) }
    };

    //attacking -> defending
    private static Dictionary<string, string> EffectivenessMap = new Dictionary<string, string>()
    {
        { "vanilla", "cacao"},
        { "cacao", "grain"},
        {"grain", "fruity"},
        {"fruity", "spicy"},
        {"spicy", "vanilla"}
    };

    public static Dictionary<string, int> RarityMap = new Dictionary<string, int>() {
        {"common", 50},
        {"rare", 30},
        {"epic", 15},
        {"legendary", 5}
    };

    [SerializeField]private List<GameObject> itemsTier1 = new List<GameObject>(){};
    [SerializeField]private List<GameObject> itemsTier2 = new List<GameObject>(){};
    [SerializeField]private List<GameObject> itemsTier3 = new List<GameObject>(){};
    [SerializeField]private List<Passive> passivesTier1 = new List<Passive>(){};
    [SerializeField]private List<Passive> passivesTier2 = new List<Passive>(){};
    [SerializeField]private List<Passive> passivesTier3 = new List<Passive>(){};

    public List<GameObject> ItemsTier1 { get => itemsTier1; set => itemsTier1 = value; }
    public List<GameObject> ItemsTier2 { get => itemsTier2; set => itemsTier2 = value; }
    public List<GameObject> ItemsTier3 { get => itemsTier3; set => itemsTier3 = value; }
    public List<Passive> PassivesTier1 { get => passivesTier1; set => passivesTier1 = value; }
    public List<Passive> PassivesTier2 { get => passivesTier2; set => passivesTier2 = value; }
    public List<Passive> PassivesTier3 { get => passivesTier3; set => passivesTier3 = value; }

    void Start() {
        ProjectileStore = GameObject.Find("ProjectileStore");
        ItemStore = GameObject.Find("ItemStore");
        EffectStore = GameObject.Find("EffectStore");
        SoundPrefab = inspectorSoundPrefab;
    }

    public static bool IsEffective(string attacking, string defending) {
        if (!EffectivenessMap.ContainsKey(attacking)) return false;
        return EffectivenessMap[attacking] == defending;
    }
    
    public static void PlayParticle(GameObject gameObject, Vector2 position) {
        GameObject particle = Instantiate(gameObject, position, Quaternion.identity);
        particle.transform.SetParent(EffectStore.transform);
        Destroy(particle, 5f);
    }

    public static void PlaySound(AudioClip audioClip, Vector2 position) {
        GameObject sound = Instantiate(SoundPrefab, position, Quaternion.identity);
        AudioSource audioSource = sound.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        sound.transform.SetParent(EffectStore.transform);
        Destroy(sound, 5f);
    }
}
