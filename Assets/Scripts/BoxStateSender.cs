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
        }
        else if (other.tag == "Player2")
        {
            onPlayer?.Invoke(boxIndex, 1, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player1")
        {
            onPlayer?.Invoke(boxIndex, 0, false);
        }
        else if (other.tag == "Player2")
        {
            onPlayer?.Invoke(boxIndex, 1, false);
        }
    }
}
