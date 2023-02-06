using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRb2d;
    private PlayerInput playerInput;
    private PlayerControls playerControls;
    [SerializeField] private float movementSpeed = 1f;

    private void Awake()
    {
        playerRb2d = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.Dash.performed += Dash;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        Debug.Log("Dashed! " + context.phase);
        //playerRb2d.AddForce(new Vector2(0, 1000));            //Todo add dash
    }

    public void Move()
    {
        playerControls.Player.Move.ReadValue<Vector2>();
        Vector2 inputVector = playerControls.Player.Move.ReadValue<Vector2>();
        playerRb2d.velocity = new Vector2(inputVector.x*movementSpeed, inputVector.y * movementSpeed);
    }
}
