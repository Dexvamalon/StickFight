using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class petScript : MonoBehaviour
{

    private bool _isPressing;
    private bool _playerHere = false;
    public GameObject interactIcon;
    public GameObject PetIcon;


    private void Start()
    {
        interactIcon.SetActive(false);
        PetIcon.SetActive(false);

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerHere = true;
            OpenInteractableIcon();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerHere = false;
            CloseInteractableIcon();
        }
    }


    private void Update()
    {
        _isPressing = Input.GetButton("attack");

        if (_playerHere && _isPressing)
        {
            PetIcon.SetActive(true);
        }
    }

    public void OpenInteractableIcon()
    {
        interactIcon.SetActive(true);

    }

    public void CloseInteractableIcon()
    {
        interactIcon.SetActive(false);

    }
}
