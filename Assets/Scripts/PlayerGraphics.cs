using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{

    PlayerMovement playerMovement;
    PlayerHealth playerHealth;
    PlayerHealth otherPlayerHealth;
    Animator playerAnimator;

    GameObject playerSword;
    DontDestroyOnLoad ddol;

    private void Awake()
    {
        playerAnimator = GetComponentInParent<Animator>();
        playerMovement = transform.root.GetComponent<PlayerMovement>();
        playerHealth = transform.parent.GetComponentInChildren<PlayerHealth>();
        if(this.tag == "Player1")
        {
            otherPlayerHealth = GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<PlayerHealth>();
        }
        else
        {
            otherPlayerHealth = GameObject.FindGameObjectWithTag("Player1").GetComponentInChildren<PlayerHealth>();
        }

        playerSword = transform.Find("Weapon").gameObject;
    }

    private void Start()
    {
        playerMovement.OnAttack += PlayerMovement_OnAttack;
        playerMovement.OnNeutralAttack += PlayerMovement_OnNeutralAttack;
        playerMovement.OnRunning += PlayerMovement_OnRunning;
        playerMovement.OnFacing += PlayerMovement_OnFacing;
        playerMovement.OnDash += PlayerMovement_OnDash;
        playerHealth.onStockLost += PlayerMovement_OnStockLost;
        otherPlayerHealth.onStockLost += PlayerMovement_OnStockLost;

        playerMovement.OnSwordPickUp += PlayerMovement_OnSwordPickUp;

        ddol = FindObjectOfType<DontDestroyOnLoad>();
    }

    private void PlayerMovement_OnSwordPickUp(string player, float pickUpDelay)
    {
        StartCoroutine(PickupWait(pickUpDelay));
    }

    private IEnumerator PickupWait(float delay)
    {
        yield return new WaitForSeconds(delay);

        playerSword.SetActive(true);
        playerAnimator.SetBool("Has sword", true);
        playerMovement.hasSword = true;
    }

    private void PlayerMovement_OnStockLost(string player)
    {
        if(transform.root.tag == player)
        {
            playerSword.SetActive(false);
            playerAnimator.SetBool("Has sword", false);
            playerMovement.hasSword = false;
        }
        else
        {
            playerSword.SetActive(true);
            playerAnimator.SetBool("Has sword", true);
            playerMovement.hasSword = true;
        }
    }

    private void PlayerMovement_OnAttack()
    {
        if(transform.root.tag == "Player1")
        {
            ddol.attackAmount++;
        }
        else
        {
            ddol.attackAmount2++;
        }
        playerAnimator.SetTrigger("Attack");
        playerAnimator.SetBool("Is running", false);
    }

    private void PlayerMovement_OnNeutralAttack()
    {
        if (transform.root.tag == "Player1")
        {
            ddol.attackAmount++;
        }
        else
        {
            ddol.attackAmount2++;
        }
        playerAnimator.SetTrigger("Neutral attack");
        playerAnimator.SetBool("Is running", false);
    }

    private void PlayerMovement_OnRunning(bool active)
    {
        if (active)
        {
            playerAnimator.SetBool("Is running", true);
        }
        else
        {
            playerAnimator.SetBool("Is running", false);
        }
    }

    private void PlayerMovement_OnFacing(Vector2 facingDir)
    {
        playerAnimator.SetBool("Is facing down", false);
        playerAnimator.SetBool("Is facing up", false);

        if (facingDir.y == 1)
        {
            playerAnimator.SetBool("Is facing up", true);
        }
        else if (facingDir.y == -1)
        {
            playerAnimator.SetBool("Is facing down", true);
        }
        else if (facingDir.x == 1)
        {
            transform.root.localScale = new Vector3(1, 1, 1); 
        }
        else if (facingDir.x == -1)
        {
            transform.root.localScale = new Vector3(-1, 1, 1);
        }

    }

    private void PlayerMovement_OnDash()
    {
        if (transform.root.tag == "Player1")
        {
            ddol.dodgeAmount++;
        }
        else
        {
            ddol.dodgeAmount2++;
        }
        playerAnimator.SetTrigger("Dodge");
        playerAnimator.SetBool("Is running", false);
    }

}
