using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    #region info

    //player1
    public int skin;
    public int weapon;
    public float dmgDealt;
    public float dmgRecieved;
    public int attackAmount;
    public int dodgeAmount;
    public int specialAmount;
    public float distanceMoved;
    public int stocksLeft;
    public float timeUnarmed;

    //player2
    public int skin2;
    public int weapon2;
    public float dmgDealt2;
    public float dmgRecieved2;
    public int attackAmount2;
    public int dodgeAmount2;
    public int specialAmount2;
    public float distanceMoved2;
    public int stocksLeft2;
    public float timeUnarmed2;

    //other
    public float matchTime;
    public bool player1Win;

    public float mainVolume;
    public float sfxVolume;
    public float musicVolume;

    #endregion

    public void ResetVariables()
    {
        dmgDealt = 0;
        dmgRecieved = 0;
        attackAmount = 0;
        dodgeAmount = 0;
        specialAmount = 0;
        distanceMoved = 0;
        stocksLeft = 0;
        timeUnarmed = 0;

        dmgDealt2 = 0;
        dmgRecieved2 = 0;
        attackAmount2 = 0;
        dodgeAmount2 = 0;
        specialAmount2 = 0;
        distanceMoved2 = 0;
        stocksLeft2 = 0;
        timeUnarmed2 = 0;

        matchTime = 0;
    }

    static DontDestroyOnLoad instance;

    private void Awake()
    {
        ManageSingelton();
    }

    private void ManageSingelton()
    {
        if(instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
