using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDowns : MonoBehaviour
{
    public bool canDash { get; private set; } = true;
    public bool canAttack { get; private set; } = true;

    private bool _canNeutralAttack = true;
    public bool canNeutralAttack
    { 
        get { return _canNeutralAttack; } 
        private set { _canNeutralAttack = value; }
    }

    public bool canMove { get; private set; } = true;

    [Header("Counted from end of action")]
    [SerializeField] private float dashCdAfterDash = 1f;
    [SerializeField] private float attackCdAfterDash = 1f;
    [SerializeField] private float neutralCdAfterDash = 1f;
    [SerializeField] private float MoveCdAfterDash = 1f;
    [Header("Counted from start of action")]
    [SerializeField] private float dashCdAfterAttack = 1f;
    [SerializeField] private float attackCdAfterAttack = 1f;
    [SerializeField] private float neutralCdAfterAttack = 1f;
    [SerializeField] private float MoveCdAfterAttack = 1f;
    [SerializeField] private float dashCdAfterNeutral = 1f;
    [SerializeField] private float attackCdAfterNeutral = 1f;
    [SerializeField] private float neutralCdAfterNeutral = 1f;
    [SerializeField] private float MoveCdAfterNeutral = 1f;

    private int mainIndex;

    private void Start()
    {
        Debug.Log(canNeutralAttack);
    }

    public void DashCoolDown()
    {
        canDash = false;
        canAttack = false;
        canNeutralAttack = false;
        canMove = false;

        mainIndex = 0;

        StartCoroutine(CoolDown(dashCdAfterDash, "canDash",0));
        StartCoroutine(CoolDown(attackCdAfterDash, "canAttack",0));
        StartCoroutine(CoolDown(neutralCdAfterDash, "canNeutralAttack",0));
        StartCoroutine(CoolDown(MoveCdAfterDash, "canMove",0));
    }

    public void AttackCoolDown()
    {
        canDash = false;
        canAttack = false;
        canNeutralAttack = false;
        canMove = false;

        mainIndex = 1;

        StartCoroutine(CoolDown(dashCdAfterAttack, "canDash",1));
        StartCoroutine(CoolDown(attackCdAfterAttack, "canAttack",1));
        StartCoroutine(CoolDown(neutralCdAfterAttack, "canNeutralAttack",1));
        StartCoroutine(CoolDown(MoveCdAfterAttack, "canMove",1));
    }
    float y;
    public void NeutralAttackCoolDown()
    {
        canDash = false;
        canAttack = false;
        canNeutralAttack = false;
        canMove = false;
        y = Time.time;

        mainIndex = 2;

        StartCoroutine(CoolDown(dashCdAfterNeutral, "canDash",2));
        StartCoroutine(CoolDown(attackCdAfterNeutral, "canAttack",2));
        StartCoroutine(CoolDown(neutralCdAfterNeutral, "canNeutralAttack",2));
        StartCoroutine(CoolDown(MoveCdAfterNeutral, "canMove",2));

    }
    int x =0;
    public IEnumerator CoolDown(float timer, string name, int coroutineIndex)
    {
        yield return new WaitForSeconds(timer);
        
        switch (name)
        {
            case "canDash":
                if (coroutineIndex == mainIndex)
                { canDash = true; }
                break;

            case "canAttack":
                if (coroutineIndex == mainIndex)
                { canAttack = true; }
                break;

            case "canNeutralAttack":
                if (coroutineIndex == mainIndex)
                { canNeutralAttack = true;
                    x++;
                    Debug.Log(x);
                    Debug.Log(Time.time - y);
                }
                break;

            case "canMove":
                if (coroutineIndex == mainIndex)
                { canMove = true; }
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
