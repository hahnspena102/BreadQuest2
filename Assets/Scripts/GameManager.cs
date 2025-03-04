using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameObject PopupStore;
    public static GameObject ProjectileStore;

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

    void Start() {
        PopupStore = GameObject.Find("PopupStore");
        ProjectileStore = GameObject.Find("ProjectileStore");
    }

    public static bool IsEffective(string attacking, string defending) {
        if (!EffectivenessMap.ContainsKey(attacking)) return false;
        return EffectivenessMap[attacking] == defending;
    }
}
