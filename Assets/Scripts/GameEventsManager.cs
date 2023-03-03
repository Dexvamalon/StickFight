using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{

    private GameObject player1;
    private GameObject player2;
    [SerializeField] private Vector3 player1StartPos;
    [SerializeField] private Vector3 player2StartPos;

    private void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        player1.GetComponentInChildren<PlayerHealth>().onStockLost += OnStockLost;
        player2.GetComponentInChildren<PlayerHealth>().onStockLost += OnStockLost;

        player1StartPos = player1.transform.position;
        player2StartPos = player2.transform.position;
    }

    private void OnStockLost(string player)
    {
        //make cooler and smother
        player1.transform.position = player1StartPos;
        player2.transform.position = player2StartPos;

        //have weapons do stuff and stuff.
    }
}
