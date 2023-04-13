using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStateSender : MonoBehaviour
{
    public event Action<int, int, bool> onPlayer; //<box index, player index(0,1), player enter>
    [SerializeField] private int boxIndex;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player1")
        {
            onPlayer?.Invoke(boxIndex, 0, true);
            GetComponentInChildren<BoxSpriteFliper>()?.SetSprite(1);
        }
        else if (other.tag == "Player2")
        {
            onPlayer?.Invoke(boxIndex, 1, true);
            GetComponentInChildren<BoxSpriteFliper>()?.SetSprite(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player1")
        {
            onPlayer?.Invoke(boxIndex, 0, false);
            GetComponentInChildren<BoxSpriteFliper>()?.SetSprite(0);
        }
        else if (other.tag == "Player2")
        {
            onPlayer?.Invoke(boxIndex, 1, false);
            GetComponentInChildren<BoxSpriteFliper>()?.SetSprite(0);
        }
    }
}
