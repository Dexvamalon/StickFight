using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private int initialStocks = 3;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            //todo //set stock and stuff.
        }
    }
}
