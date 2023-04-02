using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [HideInInspector] public float health { get; private set; }
    public float maxHealth { get; private set; } = 100f;
    [SerializeField] private float incicibilityDuration;
    private bool _indefatigable = false;
    public bool dodged = false;

    [HideInInspector] public int stocks { get; private set; }
    private int maxStocks = 3;

    public event Action<string> onStockLost;
    public event Action onPlayerDeath;

    DontDestroyOnLoad ddol;
    private CoolDowns coolDowns;

    private void Start()
    {
        health = maxHealth;
        stocks = maxStocks;

        ddol = FindObjectOfType<DontDestroyOnLoad>();
        coolDowns = transform.root.GetComponent<CoolDowns>();

        if (transform.root.tag == "Player1")
        {
            ddol.stocksLeft = stocks;
        }
        else
        {
            ddol.stocksLeft2 = stocks;
        }
    }

    public IEnumerator Invicibility()
    {
        _indefatigable = true;
        yield return new WaitForSeconds(incicibilityDuration);
        _indefatigable = false;
    }

    public void TakeDamage(float damage)
    {
        if (!_indefatigable && !dodged)
        { 
            health -= damage;
            StartCoroutine(Invicibility());
            coolDowns.HitCoolDown();
            Debug.Log(gameObject.name + " took " + damage + " damage");

            if (transform.root.tag == "Player1")
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
            else if (health <= 0 && stocks <= 1)
            {
                onPlayerDeath?.Invoke();
                if (transform.root.tag == "Player1")
                {
                    ddol.player1Win = false;
                    ddol.stocksLeft = stocks;
                }
                else
                {
                    ddol.player1Win = true;
                    ddol.stocksLeft2 = stocks;
                }
            }
        }
    }
}
