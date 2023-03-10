using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [HideInInspector] public float health { get; private set; }
    [SerializeField] private float maxHealth = 100f;

    [HideInInspector] public int stocks { get; private set; }
    private int maxStocks = 3;

    public event Action<string> onStockLost;
    public event Action onPlayerDeath;

    DontDestroyOnLoad ddol;

    private void Start()
    {
        health = maxHealth;
        stocks = maxStocks;

        ddol = FindObjectOfType<DontDestroyOnLoad>();

        if (transform.root.tag == "Player1")
        {
            ddol.stocksLeft = stocks;
        }
        else
        {
            ddol.stocksLeft2 = stocks;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage");

        if(transform.root.tag == "Player1")
        {
            ddol.dmgRecieved += damage;
        }
        else
        {
            ddol.dmgRecieved2 += damage;
        }

        if (health <= 0 && stocks > 1)
        {
            onStockLost?.Invoke(transform.root.tag);

            stocks--;
            health = maxHealth;

            if (transform.root.tag == "Player1")
            {
                ddol.stocksLeft = stocks;
            }
            else
            {
                ddol.stocksLeft2 = stocks;
            }

            Debug.Log(gameObject.name + " is dead");
        }
        else if (stocks <= 1)
        {
            onPlayerDeath?.Invoke();
            if (transform.root.tag == "Player1")
            {
                ddol.player1Win = false;
            }
            else
            {
                ddol.player1Win = true;
            }
        }
    }
}
