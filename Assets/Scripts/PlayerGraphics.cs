using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{

    PlayerMovement playerMovement;
    Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponentInParent<Animator>();
        playerMovement = transform.root.GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        playerMovement.OnAttack += PlayerMovement_OnAttack;
        playerMovement.OnNeutralAttack += PlayerMovement_OnNeutralAttack;
        playerMovement.OnRunning += PlayerMovement_OnRunning;
        playerMovement.OnFacing += PlayerMovement_OnFacing;
        playerMovement.OnDash += PlayerMovement_OnDash;
    }


    private void PlayerMovement_OnAttack()
    {
        playerAnimator.SetTrigger("Attack");
    }

    private void PlayerMovement_OnNeutralAttack()
    {
        playerAnimator.SetTrigger("Neutral attack");
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
            
    }

    private void PlayerMovement_OnDash()
    {
        playerAnimator.SetTrigger("Dodge");
    }

}
