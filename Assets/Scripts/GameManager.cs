using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameObject PopupStore;
    public static GameObject ProjectileStore;

    void Start() {
        PopupStore = GameObject.Find("PopupStore");
        ProjectileStore = GameObject.Find("ProjectileStore");
    }
}
