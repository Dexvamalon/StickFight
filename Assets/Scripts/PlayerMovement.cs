using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
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

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.drag = defaultDrag;
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

        _sr.sortingOrder = -Mathf.FloorToInt(transform.position.z * 100);
    }

    public void Move()
    {
        playerControls.Player.Move.ReadValue<Vector2>();
        Vector2 inputVector = playerControls.Player.Move.ReadValue<Vector2>();
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
            }
            else if (inputVector.y != attackDir.y)
            {
                attackDir = new Vector2(0, inputVector.y);
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
        playerRb.velocity = new Vector3(facingDir.x * dashSpeed, 0.0f, facingDir.y * dashSpeed);
        playerRb.drag = dashDrag;
        isDashing = true;

        yield return new WaitForSeconds(dashTime);

        playerRb.drag = defaultDrag;
        isDashing = false;

        coolDowns.DashCoolDown();
    }
}
