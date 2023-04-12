using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchStatsFiller : MonoBehaviour
{
    [Header("Player 1")]
    [SerializeField] TextMeshProUGUI dmgDealtSquare;
    [SerializeField] TextMeshProUGUI dmgRecievedSquare;
    [SerializeField] TextMeshProUGUI attackAmountSquare;
    [SerializeField] TextMeshProUGUI dodgeAmountSquare;
    [SerializeField] TextMeshProUGUI specialAmountSquare;
    [SerializeField] TextMeshProUGUI distanceMovedSquare;
    [SerializeField] TextMeshProUGUI stocksLeftSquare;
    [SerializeField] TextMeshProUGUI timeUnarmedSquare;
    [Header("Player 2")]
    [SerializeField] TextMeshProUGUI dmgDealt2Square;
    [SerializeField] TextMeshProUGUI dmgRecieved2Square;
    [SerializeField] TextMeshProUGUI attackAmount2Square;
    [SerializeField] TextMeshProUGUI dodgeAmount2Square;
    [SerializeField] TextMeshProUGUI specialAmount2Square;
    [SerializeField] TextMeshProUGUI distanceMoved2Square;
    [SerializeField] TextMeshProUGUI stocksLeft2Square;
    [SerializeField] TextMeshProUGUI timeUnarmed2Square;
    [Header("Extra")]
    [SerializeField] GameObject win;
    [SerializeField] GameObject win2;
    [SerializeField] TextMeshProUGUI matchTimeSquare;

    [SerializeField] Image player1Image;
    [SerializeField] Image player2Image;
    [SerializeField] Sprite[] playerIcons;

    DontDestroyOnLoad ddol;

    string matchTime;

    private void Start()
    {
        ddol = FindObjectOfType<DontDestroyOnLoad>();

        dmgDealtSquare.text = ddol.dmgDealt.ToString();
        dmgRecievedSquare.text = ddol.dmgRecieved.ToString();
        attackAmountSquare.text = ddol.attackAmount.ToString();
        dodgeAmountSquare.text = ddol.dodgeAmount.ToString();
        specialAmountSquare.text = ddol.specialAmount.ToString();
        distanceMovedSquare.text = ddol.distanceMoved.ToString();
        stocksLeftSquare.text = ddol.stocksLeft.ToString();
        timeUnarmedSquare.text = ddol.timeUnarmed.ToString();

        dmgDealt2Square.text = ddol.dmgDealt2.ToString();
        dmgRecieved2Square.text = ddol.dmgRecieved2.ToString();
        attackAmount2Square.text = ddol.attackAmount2.ToString();
        dodgeAmount2Square.text = ddol.dodgeAmount2.ToString();
        specialAmount2Square.text = ddol.specialAmount2.ToString();
        distanceMoved2Square.text = ddol.distanceMoved2.ToString();
        stocksLeft2Square.text = ddol.stocksLeft2.ToString();
        timeUnarmed2Square.text = ddol.timeUnarmed2.ToString();

        player1Image.sprite = playerIcons[ddol.skin];
        player2Image.sprite = playerIcons[ddol.skin2];

        if (Mathf.FloorToInt(ddol.matchTime / 60) == 0)
        {
            matchTime = (Mathf.RoundToInt(ddol.matchTime)).ToString();
        }
        else
        {
            matchTime = (Mathf.FloorToInt(ddol.matchTime / 60)).ToString() + ":" + (Mathf.RoundToInt(ddol.matchTime) - Mathf.FloorToInt(ddol.matchTime / 60) * 60).ToString();
        }
        matchTimeSquare.text = matchTime;

        if (ddol.player1Win)
        {
            win.SetActive(true);
        }
        else
        {
            win2.SetActive(true);
        }
    }                         
}
