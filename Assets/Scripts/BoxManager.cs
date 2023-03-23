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
    #region map var
    [Header("map var")]

    [SerializeField] private GameObject mapScroller;
    [SerializeField] private RectTransform mapScrollRect;
    int curMap;
    [SerializeField] float mapImageWidth;
    [SerializeField] float mapPadding;
    Vector3 mapCurrentVelocity;
    [SerializeField] float moveTime;
    private float mapLastFrameYMovement;
    private Vector3 mapTargetPos;
    [SerializeField] int mapCount;
    [SerializeField] List<GameObject> mapBorders;
    [SerializeField] private List<int> mapIndex;

    #endregion

    [Header("char1select var")]

    [SerializeField] private GameObject char1Scroller;
    [SerializeField] private RectTransform char1ScrollRect;
    int curChar1;
    [SerializeField] float char1ImageWidth;
    [SerializeField] float char1Padding;
    Vector3 char1CurrentVelocity;
    private float char1LastFrameYMovement;
    private Vector3 char1TargetPos;
    [SerializeField] int char1Count;
    [SerializeField] List<GameObject> char1Borders;

    [Header("char2select var")]

    [SerializeField] private GameObject char2Scroller;
    [SerializeField] private RectTransform char2ScrollRect;
    int curChar2;
    [SerializeField] float char2ImageWidth;
    [SerializeField] float char2Padding;
    Vector3 char2CurrentVelocity;
    private float char2LastFrameYMovement;
    private Vector3 char2TargetPos;
    [SerializeField] int char2Count;
    [SerializeField] List<GameObject> char2Borders;

    MainMenu mainMenu;

    private void Start()
    {
        playerControls1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerMovement>().playerControls;
        playerControls2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerMovement>().playerControls;
        playerControls1.Control.Select.performed += OnSelect1;
        playerControls1.Player.Attack.performed += OnSelect1;
        playerControls2.Control2.Select.performed += OnSelect2;
        playerControls2.Player2.Attack.performed += OnSelect2;

        BoxStateSender[] boxesList = FindObjectsOfType<BoxStateSender>(true); //hope this works, test later.
        for (int i = 0; i < boxesList.Length; i++)
        {
            boxesList[i].onPlayer += BoxStateSender_OnPlayer;
        }

        mainMenu = GetComponent<MainMenu>();

        playerBox = new int[] { -1, -1};
        currentSlider = 2;
        tempSlider = slidersArray[currentSlider ].GetComponent<Slider>();

        curMap = 0;
        mapBorders[curMap].SetActive(true);

        curChar1 = 0;
        char1Borders[curChar1].SetActive(true);

        curChar2 = 0;
        char2Borders[curChar2].SetActive(true);
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
        inputVector[1] = playerControls2.Control2.Move.ReadValue<Vector2>();

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
                    CharMove1(inputVector[i]);
                    break;
                case 3:
                    CharMove2(inputVector[i]);
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

    private void BoxToggle(int player, ref GameObject var)
    {
        if(var.activeInHierarchy)
        {
            var.SetActive(false);
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
            var.SetActive(true);
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
        if (mapScroller.activeInHierarchy)
        {
            if (input.x != 0 && input.x != mapLastFrameYMovement)
            {
                mapBorders[curMap].SetActive(false);
                curMap = Mathf.Clamp(curMap + (int)input.x, 0, mapCount-1);// set clamp to be right
                mapBorders[curMap].SetActive(true);
                Debug.Log(curMap);
                mapTargetPos = new Vector3(-(mapImageWidth / 2 * ((curMap+1) * 2 - 1) + (curMap + 1) * mapPadding), mapScrollRect.localPosition.y, mapScrollRect.localPosition.z);
                Debug.Log(mapTargetPos);
                //StartCoroutine(MapMoveSmoothDamp());
            }
            mapScrollRect.localPosition = Vector3.SmoothDamp(mapScrollRect.localPosition, mapTargetPos, ref mapCurrentVelocity, moveTime);
            //make it select middle object
            mapLastFrameYMovement = input.x;
        }
    }

    private void CharMove1(Vector2 input)
    {
        if (char1Scroller.activeInHierarchy)
        {
            if (input.x != 0 && input.x != char1LastFrameYMovement)
            {
                char1Borders[curChar1].SetActive(false);
                curChar1 = Mathf.Clamp(curChar1 + (int)input.x, 0, char1Count - 1);// set clamp to be right
                char1Borders[curChar1].SetActive(true);
                Debug.Log(curChar1);
                char1TargetPos = new Vector3(-(char1ImageWidth / 2 * ((curChar1 + 1) * 2 - 1) + (curChar1 + 1) * char1Padding), char1ScrollRect.localPosition.y, char1ScrollRect.localPosition.z);
                Debug.Log(mapTargetPos);
                //StartCoroutine(MapMoveSmoothDamp());
            }
            char1ScrollRect.localPosition = Vector3.SmoothDamp(char1ScrollRect.localPosition, char1TargetPos, ref char1CurrentVelocity, moveTime);
            //make it select middle object
            char1LastFrameYMovement = input.x;
        }
    }

    ////////////////////////////////////////////////////////////////
    // todo link up the selected character with player skin.
    ////////////////////////////////////////////////////////////////

    private void CharMove2(Vector2 input)
    {
        if (char2Scroller.activeInHierarchy)
        {
            if (input.x != 0 && input.x != char2LastFrameYMovement)
            {
                char2Borders[curChar2].SetActive(false);
                curChar2 = Mathf.Clamp(curChar2 + (int)input.x, 0, char2Count - 1);// set clamp to be right
                char2Borders[curChar2].SetActive(true);
                Debug.Log(curChar2);
                char2TargetPos = new Vector3(-(char2ImageWidth / 2 * ((curChar2 + 1) * 2 - 1) + (curChar2 + 1) * char2Padding), char2ScrollRect.localPosition.y, char2ScrollRect.localPosition.z);
                Debug.Log(char2TargetPos);
                //StartCoroutine(MapMoveSmoothDamp());
            }
            char2ScrollRect.localPosition = Vector3.SmoothDamp(char2ScrollRect.localPosition, char2TargetPos, ref char2CurrentVelocity, moveTime);
            //make it select middle object
            char2LastFrameYMovement = input.x;
        }
    }

    private IEnumerator MapMoveSmoothDamp()
    {
        float timeStart = Time.fixedTime;
        while(timeStart + moveTime + 0.5f > Time.fixedTime)
        {
            mapScrollRect.localPosition = Vector3.SmoothDamp(mapScrollRect.localPosition, mapTargetPos, ref mapCurrentVelocity, moveTime);
            yield return null;
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
                BoxToggle(player, ref sliders);
                break;
            case 1:
                mainMenu.LoadScene(3);
                break;
            case 2:
                BoxToggle(player, ref char1Scroller);
                break;
            case 3:
                BoxToggle(player, ref char2Scroller);
                break;
            case 4:
                BoxToggle(player, ref mapScroller);
                break;
            case 5:
                mainMenu.LoadScene(mapIndex[curMap]); // make this change scenes depending on map
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
        playerControls2.Control2.Select.performed -= OnSelect2;
        playerControls2.Player2.Attack.performed -= OnSelect2;
    }
}
