using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Refrences")]
    CoolDowns coolDowns;
    [Header ("Other")]
    private Rigidbody2D playerRb2d;
    private PlayerInput playerInput;
    private PlayerControls playerControls;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float dashSpeed = 1f;
    [SerializeField] private float dashTime = 1f;
    private Vector2 facingDir = new Vector2(0,-1);
    private bool isDashing = false;
    [SerializeField] private float defaultDrag = 20f;
    [SerializeField] private float dashDrag = 5f;

    private void Awake()
    {
        playerRb2d = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        coolDowns = GetComponent<CoolDowns>();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.Dash.performed += Dash;
    }

    private void FixedUpdate()
    {
        Move();
        /*if(isDashing && playerRb2d.velocity == new Vector2(0,0)) // todo fix this
        {
            isDashing = false;
        }*/
    }

    public void Move()
    {
        playerControls.Player.Move.ReadValue<Vector2>();
        Vector2 inputVector = playerControls.Player.Move.ReadValue<Vector2>();
        if (inputVector != new Vector2(0, 0))
        {
            if (!isDashing)
            {
                playerRb2d.velocity = new Vector2(inputVector.x * movementSpeed, inputVector.y * movementSpeed);
            }

            if (inputVector.x != facingDir.x)
            {
                facingDir = new Vector2(inputVector.x, 0);
            }
            if (inputVector.y != facingDir.y)
            {
                facingDir = new Vector2(0, inputVector.y);
            }
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

        StartCoroutine(DashCalculator());
    }

    private IEnumerator DashCalculator()
    {
        playerRb2d.velocity = new Vector2(facingDir.x * dashSpeed, facingDir.y * dashSpeed);
        playerRb2d.drag = dashDrag;
        isDashing = true;

        yield return new WaitForSeconds(dashTime);

        playerRb2d.drag = defaultDrag;
        isDashing = false;

        coolDowns.DashCoolDown();
    }
}
