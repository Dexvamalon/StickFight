using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMover : MonoBehaviour
{
    [SerializeField] private List<int> playerLayers = new List<int>();

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == playerLayers[0] || collision.gameObject.layer == playerLayers[1])
        {
            if(collision.gameObject.tag == "ActivatingAttack")
            {
                //make the weapon move osmth.
            }
        }
    }
}
