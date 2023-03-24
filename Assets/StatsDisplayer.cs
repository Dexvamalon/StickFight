using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatsDisplayer : MonoBehaviour
{

    [HideInInspector] public PlayerControls playerControls { get; private set; }
    private Vector2[] inputVector = new Vector2[2];
    [SerializeField] private RectTransform[] mapScrollRect = new RectTransform[2];
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject[] OnContinues = new GameObject[2];
    private MainMenu mainMenu;

    private void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Control.Enable();
        playerControls.Control2.Enable();
        playerControls.Control.Select.performed += OnSelect;
        playerControls.Control2.Select.performed += OnSelect2;

        mainMenu = GetComponent<MainMenu>();
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        OnContinues[0].SetActive(!OnContinues[0].activeInHierarchy);
    }

    private void OnSelect2(InputAction.CallbackContext context)
    {

        OnContinues[1].SetActive(!OnContinues[1].activeInHierarchy);
    }

    private void Update()
    {
        Move();
        if (OnContinues[0].activeInHierarchy && OnContinues[1].activeInHierarchy)
        {
            mainMenu.LoadScene(0);
        }
    }

    private void Move()
    {
        inputVector[0] = playerControls.Control.Move.ReadValue<Vector2>();
        inputVector[1] = playerControls.Control2.Move.ReadValue<Vector2>();

        mapScrollRect[0].localPosition += new Vector3(0, -inputVector[0].y * Time.deltaTime * moveSpeed, 0);
        mapScrollRect[1].localPosition += new Vector3(0, -inputVector[1].y * Time.deltaTime * moveSpeed, 0); 
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        playerControls.Control.Select.performed -= OnSelect;
        playerControls.Control2.Select.performed -= OnSelect2;
    }
}
