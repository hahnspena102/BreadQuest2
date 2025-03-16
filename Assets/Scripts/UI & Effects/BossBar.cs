using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class BossBar : MonoBehaviour
{
    private GameObject boss;
    private Enemy enemy;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    void Start(){
        boss = GameObject.Find("Boss");
        if (boss != null) {
            enemy = boss.GetComponent<Enemy>();
        }
    }

    void Update(){
        if (boss == null) gameObject.SetActive(false);

        if (healthSlider != null) {
            healthSlider.value = enemy.Health;
            healthSlider.maxValue = enemy.MaxHealth;
        }

        if (healthText != null) healthText.text = enemy.Health + "/" + enemy.MaxHealth;

    }
}