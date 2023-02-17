using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceenManagement : MonoBehaviour
{
    public string sceneToLoad = "";

    private bool _isPressing;
    private bool _playerHere = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerHere = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerHere = false;
        }
    }


    private void Update()
    {
        _isPressing = Input.GetButton("attack");

        if (_playerHere && _isPressing)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

}
