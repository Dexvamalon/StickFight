using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BoxManager : MonoBehaviour
{
    private PlayerControls playerControls1;
    private PlayerControls playerControls2;
    private Vector2[] inputVector = new Vector2[2];

    [SerializeField] private GameObject[] boxes;
    private int[] playerBox;

    #region slider var
    [SerializeField] private GameObject sliders;
    [SerializeField] private GameObject[] slidersArray;
    private int currentSlider;
    private Slider tempSlider;
    private float lastFrameYMovement;
    #endregion

    [SerializeField] private GameObject menuScroller;
    private ScrollRect mapScrollRect;
    [SerializeField] private RectTransform scrollRect;
    int curMap;
    [SerializeField] float mapImageWidth;
    [SerializeField] float mapPadding;
    Vector3 currentVelocity;
    [SerializeField] float moveTime;
    private float mapLastFrameYMovement;
    private Vector3 targetPos;

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
        currentSlider = 2;
        tempSlider = slidersArray[currentSlider ].GetComponent<Slider>();

        mapScrollRect = menuScroller.GetComponentInChildren<ScrollRect>();
        curMap = 1;
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
                    MapMove(inputVector[i]);
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
            if(input.y < 0 && input.y != lastFrameYMovement)
            {
                currentSlider = Mathf.Clamp(currentSlider-1, 0, 2);
                Debug.Log(currentSlider);
                tempSlider.transform.Find("Handle Slide Area").GetComponentInChildren<Image>().color = Color.white;
                tempSlider = slidersArray[currentSlider].GetComponent<Slider>();
                tempSlider.transform.Find("Handle Slide Area").GetComponentInChildren<Image>().color = new Color(0.5f, 1, 1);
            }
            if (input.y > 0 && input.y != lastFrameYMovement)
            {
                currentSlider = Mathf.Clamp(currentSlider+1, 0, 2);
                Debug.Log(currentSlider);
                tempSlider.transform.Find("Handle Slide Area").GetComponentInChildren<Image>().color = Color.white;
                tempSlider = slidersArray[currentSlider].GetComponent<Slider>();
                tempSlider.transform.Find("Handle Slide Area").GetComponentInChildren<Image>().color = new Color(0.5f, 1, 1);
            }
            if(input.x != 0)
            {
                tempSlider.value = tempSlider.value + input.x;
            }
            lastFrameYMovement = input.y;
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

    private void MapMove(Vector2 input)
    {
        if (menuScroller.activeInHierarchy)
        {
            if (input.x != 0 && input.x != mapLastFrameYMovement)
            {
                curMap = Mathf.Clamp(curMap + (int)input.x, 1, 4);// set clamp to be right
                Debug.Log(curMap);
                targetPos = new Vector3(-(mapImageWidth / 2 * (curMap * 2 - 1) + curMap * mapPadding), scrollRect.localPosition.y, scrollRect.localPosition.z);
                Debug.Log(targetPos);
                //StartCoroutine(MapMoveSmoothDamp());
            }
            scrollRect.localPosition = Vector3.SmoothDamp(scrollRect.localPosition, targetPos, ref currentVelocity, moveTime);
            //make it select middle object
            mapLastFrameYMovement = input.x;
        }
    }

    private IEnumerator MapMoveSmoothDamp()
    {
        float timeStart = Time.fixedTime;
        while(timeStart + moveTime + 0.5f > Time.fixedTime)
        {
            scrollRect.localPosition = Vector3.SmoothDamp(scrollRect.localPosition, targetPos, ref currentVelocity, moveTime);
            yield return null;
        }
    }

    private void MapToggle(int player)
    {
        if (menuScroller.activeInHierarchy)
        {
            menuScroller.SetActive(false);
            if (player == 0)
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
            menuScroller.SetActive(true);
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
        //bool i = () => { var v = context.started == true; }
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
                MapToggle(player);
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
