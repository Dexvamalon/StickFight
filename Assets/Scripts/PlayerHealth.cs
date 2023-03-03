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

    private void Awake()
    {
        health = maxHealth;
        stocks = maxStocks;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage");
        if (health <= 0 && stocks > 1)
        {
            onStockLost?.Invoke(transform.root.tag);
            stocks--;
            health = maxHealth;

            Debug.Log(gameObject.name + " is dead");
        }
        else if (stocks <= 1)
        {
            //Send to after game screen
        }
    }
}
