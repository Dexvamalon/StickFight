using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100;
    //[SerializeField] private int initialStocks = 3;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage");
        if (health <= 0)
        {
            Debug.Log(gameObject.name + " is dead");
            //todo //set stock and stuff.
            //fire of losing swor event osmth
        }
    }
}
