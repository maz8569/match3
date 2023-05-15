using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesiredDish : MonoBehaviour
{
    [SerializeField]
    private Client _parent;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer.sprite = _parent._desiredDish.Sprite;
    }

    public void Init()
    {
        
    }
    
}
