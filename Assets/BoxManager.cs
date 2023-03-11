using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoxManager : MonoBehaviour
{
    private PlayerControls playerControls1;
    private PlayerControls playerControls2;
    private Vector2[] inputVector = new Vector2[2];

    [SerializeField] private GameObject[] boxes;
    private int[] playerBox;

    [SerializeField] GameObject sliders;

    MainMenu mainMenu;

    private void Start()
    {
        playerControls1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerMovement>().playerControls;
        //playerControls2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerMovement>().playerControls;
        playerControls1.Control.Select.performed += OnSelect1;
        playerControls1.Player.Attack.performed += OnSelect1;
        //playerControls2.Control2.Select.performed += OnSelect2;
        //playerControls2.Player2.Attack.performed += OnSelect2;

        BoxStateSender[] boxesList = FindObjectsOfType<BoxStateSender>(true); //hope this works, test later.
        for (int i = 0; i < boxesList.Length; i++)
        {
            boxesList[i].onPlayer += BoxStateSender_OnPlayer;
        }

        mainMenu = GetComponent<MainMenu>();

        playerBox = new int[] { -1, -1};
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void BoxStateSender_OnPlayer(int box,int player,bool action)
    {
        boxes[box].SetActive(action);
        if(action)
        {
            playerBox[player] = box;
        }
        else
        {
            playerBox[player] = -1;
        }
    }

    private void Move()
    {
        inputVector[0] = playerControls1.Control.Move.ReadValue<Vector2>();
        //inputVector[1] = playerControls2.Control2.Move.ReadValue<Vector2>();

        for(int i = 0; i < playerBox.Length; i++)
        {
            switch (playerBox[i])
            {
                case 0:
                    SettingsMove(inputVector[i]);
                    break;
                case 1:

                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
                default:
                    break;


            }
        }

    }

    private void SettingsMove(Vector2 input)
    {
        if (sliders.activeInHierarchy)
        {
            //do slider stuff
        }
    }

    private void SettingsToggle(int player)
    {
        if(sliders.activeInHierarchy)
        {
            sliders.SetActive(false);
            if(player == 0)
            {
                playerControls1.Control.Disable();
                playerControls1.Player.Enable();
            }
            else
            {
                playerControls2.Control2.Disable();
                playerControls2.Player2.Enable();
            }
        }
        else
        {
            sliders.SetActive(true);
            if (player == 0)
            {
                playerControls1.Control.Enable();
                playerControls1.Player.Disable();
            }
            else
            {
                playerControls2.Control2.Enable();
                playerControls2.Player2.Disable();
            }
        }
    }

    private void OnSelect1(InputAction.CallbackContext context)
    {
        OnSelect(0);
    }
    private void OnSelect2(InputAction.CallbackContext context)
    {
        OnSelect(1);
    }
    private void OnSelect(int player)
    {
        switch (playerBox[player])
        {
            case 0:
                SettingsToggle(player);
                break;
            case 1:
                mainMenu.LoadScene(3);
                break;
            case 2:

                break;
            case 3:

                break;
            case 4:
                mainMenu.LoadScene(1);
                break;
            default:
                break;


        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
        playerControls1.Control.Select.performed -= OnSelect1;
        playerControls1.Player.Attack.performed -= OnSelect1;
    }
}
