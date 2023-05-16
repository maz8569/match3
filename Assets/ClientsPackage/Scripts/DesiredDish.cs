using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesiredDish : MonoBehaviour
{
    private Camera _mainCamera;

    [SerializeField]
    private Client _parent;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _spriteRenderer.sprite = _parent._desiredDish.Sprite;
    }

    public void OnTap(InputAction.CallbackContext context) //TODO: one touch handler
    {
        if(!context.started) return;

        Vector2 touchPosition;

        #if UNITY_ANDROID || UNITY_IOS
            touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        #elif UNITY_STANDALONE
            touchPosition = Mouse.current.position.ReadValue();
        #endif

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(touchPosition));
        if (!rayHit.collider) return;
        
        Debug.Log(rayHit.collider.gameObject.name);
    }
    
}
