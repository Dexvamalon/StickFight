using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class petScript : MonoBehaviour
{
    private bool _isPressing;
    private bool _playerHere = false;
    public GameObject interactIcon;


    // Start is called before the first frame update
    void Start()
    {
        interactIcon.SetActive(false);
    }

    private void Update()
    {
        _isPressing = Input.GetButton("attack");

        if (_playerHere && _isPressing)
        {
            GetComponent<sceenManagement>().OpenInteractableIcon();
        }

        /* else
        {
            GetComponent<sceenManagement>().CloseInteractableIcon();

        } */
    }
}
