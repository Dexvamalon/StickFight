using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Slider player1HealthSlider;
    [SerializeField] private Slider player2HealthSlider;
    [SerializeField] private Slider player1StockSlider;
    [SerializeField] private Slider player2StockSlider;


    PlayerHealth player1HealthScript;
    PlayerHealth player2HealthScript;

    private void Start()
    {
        player1HealthScript = GameObject.FindGameObjectWithTag("Player1").GetComponentInChildren<PlayerHealth>();
        player2HealthScript = GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<PlayerHealth>();

        player1HealthSlider.maxValue = player1HealthScript.maxHealth;
        player1HealthSlider.value = player1HealthScript.maxHealth;
        player2HealthSlider.maxValue = player2HealthScript.maxHealth;
        player2HealthSlider.value = player2HealthScript.maxHealth;

    }

    private void FixedUpdate()
    {
        if(player1HealthScript.health < player1HealthSlider.value)
        {
            player1HealthSlider.value--;
            if (player1HealthScript.health > player1HealthSlider.value)
            {
                player1HealthSlider.value = player1HealthScript.health;
            }
        }
        else if (player1HealthScript.health > player1HealthSlider.value)
        {
            player1HealthSlider.value = player1HealthScript.health;
        }

        if (player2HealthScript.health < player2HealthSlider.value)
        {
            player2HealthSlider.value--;
            if (player2HealthScript.health > player2HealthSlider.value)
            {
                player2HealthSlider.value = player2HealthScript.health;
            }
        }
        else if (player2HealthScript.health > player2HealthSlider.value)
        {
            player2HealthSlider.value = player2HealthScript.health;
        }

        player1StockSlider.value = player1HealthScript.stocks;
        player2StockSlider.value = player2HealthScript.stocks;
    }
}
