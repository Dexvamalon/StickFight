using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpriteFliper : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites;

    SpriteRenderer _sr;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(int sprite)
    {
        _sr.sprite = sprites[sprite];
    }
}
