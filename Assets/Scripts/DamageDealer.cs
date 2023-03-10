using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private float damage = 10;
    [SerializeField] private List<int> playerLayers = new List<int>();

    public event Action<float> OnAttackHit;
    private PlayerHealth otherHealth;

    DontDestroyOnLoad ddol;

    private void Start()
    {
        ddol = FindObjectOfType<DontDestroyOnLoad>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == playerLayers[0] || collision.gameObject.layer == playerLayers[1])
        {
            if (collision.gameObject.GetComponent<PlayerHealth>() != null && otherHealth == null)
            {
                otherHealth = collision.gameObject.GetComponent<PlayerHealth>();
                OnAttackHit += otherHealth.TakeDamage;
            }

            if(collision.gameObject.GetComponent<PlayerHealth>() != null)
            {
                OnAttackHit?.Invoke(damage);

                if(transform.root.tag == "player1")
                {
                    ddol.dmgDealt += damage;
                }
                else
                {
                    ddol.dmgDealt2 += damage;
                }
            }
        }

        // add other hits than player with parameters ig here.
    }
}
