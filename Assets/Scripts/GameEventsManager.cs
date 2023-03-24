using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{

    private GameObject player1;
    private GameObject player2;
    [SerializeField] private GameObject player1Sword;
    [SerializeField] private GameObject player2Sword;
    private Vector3 player1StartPos;
    private Vector3 player2StartPos;
    private Vector3 swordPos;
    [SerializeField] private float swordMoveTime;

    #region SwordVariables
    bool isPlayer1;
    Vector3 currentVelocity;
    #endregion

    float matchStartTime;
    DontDestroyOnLoad ddol;

    private void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        player1.GetComponentInChildren<PlayerHealth>().onStockLost += OnStockLost;
        player2.GetComponentInChildren<PlayerHealth>().onStockLost += OnStockLost;
        player1.GetComponentInChildren<PlayerHealth>().onPlayerDeath += OnPlayerDeath;
        player2.GetComponentInChildren<PlayerHealth>().onPlayerDeath += OnPlayerDeath;
        player1.GetComponent<PlayerMovement>().OnSwordPickUp += OnSwordPickUp;
        player2.GetComponent<PlayerMovement>().OnSwordPickUp += OnSwordPickUp;

        player1StartPos = player1.transform.position;
        player2StartPos = player2.transform.position;

        ddol = FindObjectOfType<DontDestroyOnLoad>();
        matchStartTime = Time.time;
    }


    private void OnSwordPickUp(string player, float x)
    {
        //make sword move toward player and then disable.
        
        if (player == "Player1")
        {
            isPlayer1 = true;
        }
        else
        {
            isPlayer1 = false;
        }
        StartCoroutine(SwordPickUpMover(player));
    }

    private IEnumerator SwordPickUpMover(string player)
    {
        float startTime = Time.fixedTime;

        if(isPlayer1)
        {
            while(startTime + swordMoveTime + 0.5f > Time.fixedTime)
            {
                player1Sword.transform.position = Vector3.SmoothDamp(player1Sword.transform.position, player1.transform.position, ref currentVelocity, swordMoveTime);
                yield return null;
            }
            player1Sword.SetActive(false);
        }
        else
        {
            while (startTime + swordMoveTime + 0.5f > Time.fixedTime)
            {
                player2Sword.transform.position = Vector3.SmoothDamp(player2Sword.transform.position, player2.transform.position, ref currentVelocity, swordMoveTime);
                yield return null;
            }
            player2Sword.SetActive(false);
        }
    }

    private void Update()
    {
        
    }

    private void OnPlayerDeath()
    {
        ddol.matchTime = Time.time - matchStartTime;
        GetComponent<MainMenu>().LoadScene(4);
    }

    private void OnStockLost(string player)
    {
        //make cooler and smother
        player1.transform.position = player1StartPos;
        player2.transform.position = player2StartPos;

        switch (player)
        {
            case "Player1":
                player1Sword.SetActive(true);
                player2Sword.SetActive(false);
                player1Sword.transform.position = swordPos;
                break;
            case "Player2":
                player1Sword.SetActive(false);
                player2Sword.SetActive(true);
                player2Sword.transform.position = swordPos;
                break;
            default:
                break;
        }
            

        //have weapons do stuff and stuff.
    }
}
