using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //add way to start attack and send attack event.
    [Header("Refrences")]
    CoolDowns coolDowns;
    [Header ("Other")]
    private Rigidbody playerRb;
    private PlayerInput playerInput;
    private PlayerControls playerControls;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float dashSpeed = 1f;
    [SerializeField] private float dashTime = 1f;
    private Vector2 facingDir = new Vector2(0,-1);
    private Vector2 attackDir = new Vector2(0, -1);
    private bool isDashing = false;
    [SerializeField] private float defaultDrag = 20f;
    [SerializeField] private float dashDrag = 5f;

    [SerializeField] private SpriteRenderer _sr;

    [SerializeField] private bool isPlayer1 = true;

    private Vector2 inputVector;

    [SerializeField] private float movementEventSpeed = 0.1f;


    //////////////////////////////////////////////////////////
                           //Events//
    //////////////////////////////////////////////////////////
    
    public event Action OnAttack;
    public event Action OnNeutralAttack;
    public event Action<bool> OnRunning;
    public event Action<Vector2> OnFacing;
    public event Action OnDash;


    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.drag = defaultDrag;
        playerInput = GetComponent<PlayerInput>();
        coolDowns = GetComponent<CoolDowns>();

        playerControls = new PlayerControls();
        if(isPlayer1)
        {   playerControls.Player.Enable();
            playerControls.Player.Dash.performed += Dash;
            playerControls.Player.Attack.performed += AttackPerformed;
        }
        else
        {   playerControls.Player2.Enable();
            playerControls.Player2.Dash.performed += Dash;
            playerControls.Player2.Attack.performed += AttackPerformed;
        }
    }

    private void FixedUpdate()
    {
        Move();

        _sr.sortingOrder = -Mathf.FloorToInt(transform.position.z * 100);
    }

    public void Move()
    {
        if (isPlayer1)
        {
            playerControls.Player.Move.ReadValue<Vector2>();
            inputVector = playerControls.Player.Move.ReadValue<Vector2>();
        }
        else
        {
            playerControls.Player2.Move.ReadValue<Vector2>();
            inputVector = playerControls.Player2.Move.ReadValue<Vector2>();
        }

        if (inputVector != new Vector2(0, 0))
        {
            if (!isDashing)
            {
                playerRb.velocity = new Vector3(inputVector.x * movementSpeed, 0.0f, inputVector.y * movementSpeed);
            }

            facingDir = inputVector;

            if (inputVector.x != attackDir.x)
            {
                attackDir = new Vector2(inputVector.x, 0);
                OnFacing?.Invoke(attackDir);
            }
            else if (inputVector.y != attackDir.y)
            {
                attackDir = new Vector2(0, inputVector.y);
                OnFacing?.Invoke(attackDir);
            }
        }

        if(Mathf.Abs(playerRb.velocity.x) > movementEventSpeed || Mathf.Abs(playerRb.velocity.z) > movementEventSpeed)
        {
            OnRunning?.Invoke(true);
        }
        else
        {
            OnRunning?.Invoke(false);
        }
    }

    public void Dash(InputAction.CallbackContext context) // todo add dash cooldown, add input buffer.
    {
        Debug.Log(context);
        Debug.Log("Dashed! " + context.phase);
        
        if (!coolDowns.canDash || isDashing)
        {
            return;
        }

        OnDash?.Invoke();
        StartCoroutine(DashCalculator());
    }

    private void AttackPerformed(InputAction.CallbackContext context)
    {
        if(inputVector == new Vector2(0, 0))
        {
            OnNeutralAttack?.Invoke();
        }
        else
        {
            OnAttack?.Invoke();
        }
        
    }

    private IEnumerator DashCalculator()
    {
        playerRb.velocity = new Vector3(facingDir.x * dashSpeed, 0.0f, facingDir.y * dashSpeed);
        playerRb.drag = dashDrag;
        isDashing = true;

        yield return new WaitForSeconds(dashTime);

        playerRb.drag = defaultDrag;
        isDashing = false;

        coolDowns.DashCoolDown();
    }
}
