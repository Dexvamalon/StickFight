using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDowns : MonoBehaviour
{
    public bool canDash { get; private set; } = true;
    [SerializeField] private float dashCdAfterDash = 1f;


    public void DashCoolDown()
    {
        Debug.Log(canDash);
        // what you can't do during cooldown
        canDash = false;
        
        StartCoroutine(CoolDown(dashCdAfterDash, "canDash"));

    }

    public IEnumerator CoolDown(float timer, string name)
    {
        yield return new WaitForSeconds(timer);
        
        switch (name)
        {
            case "canDash":
                canDash = true;
                break;
            default:
                Debug.Log("no corect variable entered!");
                Debug.Break();
                break;

        }
    }











    /*                                              !!!   Just for testing   !!!
    private bool b = true;

    private void Start()
    {
        CoolMethod(out b);
        Debug.Log(b);
    }

    private void CoolMethod(out bool a)
    {
        a = false;
    }*/
}
