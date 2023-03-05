using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMover : MonoBehaviour
{
    [SerializeField] private List<int> playerLayers = new List<int>();

    private Rigidbody swordRB;
    [SerializeField] private float launchForce;

    private void Start()
    {
        swordRB = transform.root.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == playerLayers[0] || collision.gameObject.layer == playerLayers[1])
        {
            if(collision.gameObject.tag == "ActivatingAttack")
            {
                PlayerMovement playerMovement = collision.transform.root.GetComponent<PlayerMovement>() ?? null;

                Mover(playerMovement.attackDir);
            }
        }
    }

    private void Mover(Vector2 vec)
    {
        swordRB.AddForce(new Vector3(vec.x, 0, vec.y) * launchForce, ForceMode.Impulse);
        Debug.Log(swordRB.velocity);
    }
}
