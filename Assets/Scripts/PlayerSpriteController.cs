using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    [Header("0 for down 1 for up and 2 for side")]
    [SerializeField] private int spriteIndex = 1;

    [Header("the different skins for the bodypart")]
    [SerializeField] private List<Sprite> skinDown;
    [SerializeField] private List<Sprite> skinUp;
    [SerializeField] private List<Sprite> skinSide;

    private int skinIndex = 0;

    [SerializeField] bool isPartOfSkin = true;


    private int orderInLayer = 0;
    private int parentOrderInLayer;


    SpriteRenderer parentSpriteRenderer;
    SpriteRenderer spriteRenderer;
    DontDestroyOnLoad ddol;

    private void Start()
    {
        ddol = FindObjectOfType<DontDestroyOnLoad>();

        if(isPartOfSkin)
        {
            if(transform.root.tag == "Player1")
            {
                skinIndex = ddol.skin;
            }
            else
            {
                skinIndex = ddol.skin2;
            }
        }
        else
        {
            if (transform.root.tag == "Player1")
            {
                skinIndex = ddol.weapon;
            }
            else
            {
                skinIndex = ddol.weapon2;
            }
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

        if(isPartOfSkin)
        {
            List<Sprite>[] directions = { skinDown, skinUp, skinSide };
            spriteRenderer.sprite = directions[ spriteIndex][Mathf.Clamp(skinIndex, 0, directions[spriteIndex].Count-1)];
            Debug.Log(spriteRenderer.sprite);
        }

        parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        orderInLayer = spriteRenderer.sortingOrder;
        parentOrderInLayer = parentSpriteRenderer.sortingOrder; 
    }

    private void LateUpdate()
    {
        if (spriteRenderer.sortingOrder != parentOrderInLayer + orderInLayer)
        {
            orderInLayer = spriteRenderer.sortingOrder;
        }

        parentOrderInLayer = parentSpriteRenderer.sortingOrder;
        spriteRenderer.sortingOrder = parentOrderInLayer + orderInLayer;

        if(!isPartOfSkin)
        {
            return;
        }

        List<Sprite>[] directions = { skinDown, skinUp, skinSide };
        if (spriteRenderer.sprite != directions[spriteIndex][Mathf.Clamp(skinIndex, 0, directions[spriteIndex].Count - 1)])
        {
            //Debug.Log(directions[spriteIndex][skinIndex]);
        }
        spriteRenderer.sprite = directions[spriteIndex][Mathf.Clamp(skinIndex, 0, directions[spriteIndex].Count - 1)];
    }

}
