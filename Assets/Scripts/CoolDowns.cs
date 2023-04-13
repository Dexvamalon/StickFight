using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CoolDowns : MonoBehaviour
{
    public bool canDash { get; private set; } = true;
    public bool canAttack { get; private set; } = true;

    private bool _canNeutralAttack = true;
    public bool CanNeutralAttack
    { 
        get { return _canNeutralAttack; } 
        private set { _canNeutralAttack = value; }
    }

    public bool canMove { get; private set; } = true;
    public bool canSpecial { get; private set; } = true;

    [Header("Counted from end of action")]
    [SerializeField] private float dashCdAfterDash = 1f;
    [SerializeField] private float attackCdAfterDash = 1f;
    [SerializeField] private float neutralCdAfterDash = 1f;
    [SerializeField] private float MoveCdAfterDash = 1f;
    [SerializeField] private float SpecialCdAfterDash = 1f;
    [Header("Counted from start of action")]
    [SerializeField] private float dashCdAfterAttack = 1f;
    [SerializeField] private float attackCdAfterAttack = 1f;
    [SerializeField] private float neutralCdAfterAttack = 1f;
    [SerializeField] private float MoveCdAfterAttack = 1f;
    [SerializeField] private float SpecialCdAfterAttack = 1f;
    [SerializeField] private float dashCdAfterNeutral = 1f;
    [SerializeField] private float attackCdAfterNeutral = 1f;
    [SerializeField] private float neutralCdAfterNeutral = 1f;
    [SerializeField] private float MoveCdAfterNeutral = 1f;
    [SerializeField] private float SpecialCdAfterNeutral = 1f;

    [SerializeField] private float dashCdAfterHit = 1f;
    [SerializeField] private float attackCdAfterHit = 1f;
    [SerializeField] private float neutralCdAfterHit = 1f;
    [SerializeField] private float MoveCdAfterHit = 1f;
    [SerializeField] private float SpecialCdAfterHit = 1f;

    [SerializeField] private List<PlayerSpriteController> playerSpriteControllers = new List<PlayerSpriteController>();

    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material lightMaterial;

    private int mainIndex;

    public void DashCoolDown()
    {
        canDash = false;
        canAttack = false;
        CanNeutralAttack = false;
        canMove = false;
        canSpecial = false;

        mainIndex = 0;

        StartCoroutine(CoolDown(dashCdAfterDash, "canDash",0));
        StartCoroutine(CoolDown(attackCdAfterDash, "canAttack",0));
        StartCoroutine(CoolDown(neutralCdAfterDash, "canNeutralAttack",0));
        StartCoroutine(CoolDown(MoveCdAfterDash, "canMove",0));
        StartCoroutine(CoolDown(SpecialCdAfterDash, "canSpecial", 0));
    }

    public void AttackCoolDown()
    {
        canDash = false;
        canAttack = false;
        CanNeutralAttack = false;
        canMove = false;
        canSpecial = false;

        mainIndex = 1;

        StartCoroutine(CoolDown(dashCdAfterAttack, "canDash",1));
        StartCoroutine(CoolDown(attackCdAfterAttack, "canAttack",1));
        StartCoroutine(CoolDown(neutralCdAfterAttack, "canNeutralAttack",1));
        StartCoroutine(CoolDown(MoveCdAfterAttack, "canMove",1));
        StartCoroutine(CoolDown(SpecialCdAfterAttack, "canSpecial", 1));
    }
    public void HitCoolDown()
    {
        canDash = false;
        canAttack = false;
        CanNeutralAttack = false;
        canMove = false;
        canSpecial = false;

        SetMaterial("light");

        mainIndex = 3;

        StartCoroutine(CoolDown(dashCdAfterHit, "canDash", 3));
        StartCoroutine(CoolDown(attackCdAfterHit, "canAttack", 3));
        StartCoroutine(CoolDown(neutralCdAfterHit, "canNeutralAttack", 3));
        StartCoroutine(CoolDown(MoveCdAfterHit, "canMove", 3));
        StartCoroutine(CoolDown(SpecialCdAfterHit, "canSpecial", 3));
    }
    float y;
    public void NeutralAttackCoolDown()
    {
        canDash = false;
        canAttack = false;
        CanNeutralAttack = false;
        canMove = false;
        canSpecial = false;
        y = Time.time;

        mainIndex = 2;

        StartCoroutine(CoolDown(dashCdAfterNeutral, "canDash",2));
        StartCoroutine(CoolDown(attackCdAfterNeutral, "canAttack",2));
        StartCoroutine(CoolDown(neutralCdAfterNeutral, "canNeutralAttack",2));
        StartCoroutine(CoolDown(MoveCdAfterNeutral, "canMove",2));
        StartCoroutine(CoolDown(SpecialCdAfterNeutral, "canSpecial", 2));

    }

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
                { 
                    canAttack = true;
                    SetMaterial("default");
                }
                break;

            case "canNeutralAttack":
                if (coroutineIndex == mainIndex)
                { CanNeutralAttack = true;
                    SetMaterial("default");
                }
                break;

            case "canMove":
                if (coroutineIndex == mainIndex)
                { canMove = true; }
                break;

            case "canSpecial":
                if (coroutineIndex == mainIndex)
                { canSpecial = true; }
                break;

            default:
                UnityEngine.Debug.Log("no corect variable entered!");
                break;

        }
    }

    private void SetMaterial(string material)
    {
        switch (material)
        {
            case "default":
                foreach(PlayerSpriteController i in playerSpriteControllers)
                {
                    i.SetMaterial(normalMaterial);
                }
                break;

            case "light":
                foreach (PlayerSpriteController i in playerSpriteControllers)
                {
                    i.SetMaterial(lightMaterial);
                }
                break;

            default:
                break;
        }
    }









    /*                                              !!!   Just for testing   !!!
    private bool b = true;

    private void Start()
    {
        CoolMethod(out b);
        .Log(b);
    }

    private void CoolMethod(out bool a)
    {
        a = false;
    }*/
}
