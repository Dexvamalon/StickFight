using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
        #region variables

    #region refrences

    [Header("Refrences")]
    CoolDowns coolDowns;
    private Rigidbody playerRb;
    private PlayerInput playerInput;
    [HideInInspector] public PlayerControls playerControls { get; private set; }
    [SerializeField] private SpriteRenderer _sr;
    DontDestroyOnLoad ddol;
    PlayerHealth playerHealth;

    #endregion
    #region movment

    [Header ("Other")]
    [SerializeField] private float movementSpeed = 1f;        //movement
    [SerializeField] private float coolDownSpeed = 1f;        //movement
    private bool slowMove = true;                             //mevement
    [SerializeField] private float movementEventSpeed = 0.1f; //movement
    private Vector3 lastPos;

    #endregion
    #region dashes

    [SerializeField] private float dashSpeed = 1f;            //dash
    [SerializeField] private float dashTime = 1f;             //dash
    [SerializeField] private float defaultDrag = 20f;         //dash
    [SerializeField] private float dashDrag = 5f;             //dash

    #endregion
    #region input
    [SerializeField] private bool isPlayer1 = true;           //input
    private Vector2 inputVector;                              //input

    #endregion
    #region Input buffer

    private Action inputBufferPointer;
    private IEnumerator curentInputBuffer;

    private bool waiting = false;

    [Header("Input buffers, for smother controls. It makes you able to press earlier than you can execute the input so that the input gets executed later")]
    [SerializeField] private float neutralAttackBuffer = 0.1f;
    [SerializeField] private float attackBuffer = 0.1f;
    [SerializeField] private float dashBuffer = 0.1f;
    [SerializeField] private float specialBuffer = 0.1f;

    #endregion
    #region facing direction

    private Vector2 facingDir = new Vector2(0,-1);            //dir
    [HideInInspector] public Vector2 attackDir { get; private set; } = new Vector2(0, -1); //dir

    #endregion
    #region Animator events

    public event Action OnAttack;
    public event Action OnNeutralAttack;
    public event Action<bool> OnRunning;
    public event Action<Vector2> OnFacing;
    public event Action OnDash;

    #endregion
    #region sword pickup

    [HideInInspector] public bool hasSword = true;            //sword
    [SerializeField] private float SwordPickUpDelay = 1f;     //sword
    public event Action<string, float> OnSwordPickUp;         //sword

    #endregion

    #endregion

    #region code

    private void Start()
    {

        playerControls = new PlayerControls();
        if(isPlayer1)
        {   playerControls.Player.Enable();
            playerControls.Player.Dash.performed += Dash;
            playerControls.Player.Attack.performed += AttackPerformed;
            playerControls.Player.Special.performed += SpecialPerformed;
        }
        else
        {   playerControls.Player2.Enable();
            playerControls.Player2.Dash.performed += Dash;
            playerControls.Player2.Attack.performed += AttackPerformed;
            playerControls.Player2.Special.performed += SpecialPerformed;
        }
        Start2();
    }

    private void Start2()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.drag = defaultDrag;
        playerInput = GetComponent<PlayerInput>();
        coolDowns = GetComponent<CoolDowns>();
        ddol = FindObjectOfType<DontDestroyOnLoad>();
        playerHealth = GetComponentInChildren<PlayerHealth>();

        lastPos = transform.position;
    }

    private void Update()
    {
        if (!hasSword)
        {
            if (transform.root.tag == "Player1")
            {
                ddol.timeUnarmed += Time.deltaTime;
            }
            else
            {
                ddol.timeUnarmed2 += Time.deltaTime;
            }
        }

        if (transform.root.tag == "Player1")
        {
            ddol.distanceMoved += Vector3.Distance(transform.position, lastPos);
        }
        else
        {
            ddol.distanceMoved2 += Vector3.Distance(transform.position, lastPos);
        }

        lastPos = transform.position;
    }

    public void SpecialPerformed(InputAction.CallbackContext context)
    {
        if (!coolDowns.canSpecial)
        {
            BufferCoroutineStarter(specialBuffer);
            inputBufferPointer = SpecialChecker;
            StartCoroutine(FrameChecker());
        }
        else
        {
            if (transform.root.tag == "Player1")
            {
                ddol.specialAmount++;
            }
            else
            {
                ddol.specialAmount2++;
            }

            Special();
        }
    }

    public void Special()
    {
        if (!hasSword)
        {
            OnSwordPickUp?.Invoke(transform.tag, SwordPickUpDelay);
        }
        else
        {
            //activate special attack;
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
            inputVector = playerControls.Player.Move.ReadValue<Vector2>();
        }
        else
        {
            inputVector = playerControls.Player2.Move.ReadValue<Vector2>();
        }

        if (inputVector != new Vector2(0, 0))
        {
            if (coolDowns.canMove)
            {
                playerRb.velocity = new Vector3(inputVector.x * movementSpeed, 0.0f, inputVector.y * movementSpeed);
            }
            else if (slowMove)
            {
                playerRb.velocity = new Vector3(inputVector.x * movementSpeed * coolDownSpeed, 0.0f, inputVector.y * movementSpeed * coolDownSpeed);
            }

            facingDir = inputVector;

            
            if (inputVector.x != attackDir.x && inputVector.x != 0)
            {
                attackDir = new Vector2(inputVector.x, 0);
                Vector2 vec = attackDir;
                vec.Normalize();
                attackDir = vec;
                OnFacing?.Invoke(attackDir);
                Debug.Log("Turnnnnnnn,,,,,,..... " + attackDir + " x");
            }
            else if (inputVector.y != attackDir.y && inputVector.y != 0)
            {
                attackDir = new Vector2(0, inputVector.y);
                Vector2 vec = attackDir;
                vec.Normalize();
                attackDir = vec;
                OnFacing?.Invoke(attackDir);
                Debug.Log("Turnnnnnnn,,,,,,..... " + attackDir + " y");
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

    public void Dash(InputAction.CallbackContext context) // TODO add dash cooldown, add input buffer.
    {
        //Debug.Log(context);
        //Debug.Log("Dashed! " + context.phase);
        
        if (!coolDowns.canDash)
        {
            BufferCoroutineStarter(dashBuffer);
            inputBufferPointer = DashChecker;
            StartCoroutine(FrameChecker());
        }
        else
        {
            StartDash();
        }
    }

    private void AttackPerformed(InputAction.CallbackContext context)
    {
        Debug.Log(coolDowns.CanNeutralAttack);
        //int[] nice = new int[2];
        //nice[8] = 4;

        if (inputVector == Vector2.zero)
        {
            Debug.Log(coolDowns.CanNeutralAttack);
            

            if (!coolDowns.CanNeutralAttack)
            {
                BufferCoroutineStarter(neutralAttackBuffer);
                inputBufferPointer = NeutralAttackChecker;
                StartCoroutine(FrameChecker());
            }
            else
            {
                NeutralAttack();
            }
        }
        else
        {
            if (!coolDowns.canAttack)
            {
                BufferCoroutineStarter(attackBuffer);
                inputBufferPointer = AttackChecker;
                StartCoroutine(FrameChecker());
            }
            else
            {
                Attack();
            }
        }
        
    }

    private void BufferCoroutineStarter(float buf)
    {
        waiting = true;

        /*if("InputBuffer" != null)
        {
            StopCoroutine("InputBuffer");
        }*/
        StartCoroutine(InputBuffer(buf));
    }


    int i = 0;
    private IEnumerator InputBuffer(float buffer)
    {
        Debug.Log(i++ + "wow   coiol");
        yield return new WaitForSeconds(buffer);

        waiting = false;
    }

    private IEnumerator FrameChecker()
    {
        while (waiting)
        {
            inputBufferPointer();
            yield return null;
        }
    }
    private void SpecialChecker() {
        if (coolDowns.canSpecial) {
            waiting = false;
            Special();
        }
    }

    private void AttackChecker() {
        if (coolDowns.canAttack) {
            waiting = false;
            Attack();
        }
    }

    private void NeutralAttackChecker() {
        if (coolDowns.CanNeutralAttack) {
            waiting = false;
            NeutralAttack();    
        }
    }

    private void DashChecker() {
        if (coolDowns.canDash) {
            waiting = false;
            StartDash();
        }
    }
    private void NeutralAttack()
    {
        coolDowns.NeutralAttackCoolDown();
        OnNeutralAttack?.Invoke();
    }

    private void Attack()
    {
        coolDowns.AttackCoolDown();
        OnAttack?.Invoke();
    }

    private void StartDash()
    {
        coolDowns.DashCoolDown();
        OnDash?.Invoke();
        StartCoroutine(DashCalculator());
    }

    private IEnumerator DashCalculator()
    {
        slowMove = false;
        playerRb.velocity = new Vector3(facingDir.x * dashSpeed, 0.0f, facingDir.y * dashSpeed);
        playerRb.drag = dashDrag;

        yield return new WaitForSeconds(dashTime);

        slowMove = true;
        playerRb.drag = defaultDrag;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        playerControls.Player.Dash.performed -= Dash;
        playerControls.Player.Attack.performed -= AttackPerformed;
        playerControls.Player.Special.performed -= SpecialPerformed;
        playerControls.Player2.Dash.performed -= Dash;
        playerControls.Player2.Attack.performed -= AttackPerformed;
        playerControls.Player2.Attack.performed -= SpecialPerformed;
    }

    #endregion
}