using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchManager : MonoBehaviour
{
    public event EventHandler<Vector3> OnTap;
    public event EventHandler<Vector3> OnDrag;
    public event EventHandler OnDragCancelled;

    private PlayerInput playerInput;

    private InputAction tapAction;
    private InputAction dragAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        tapAction = playerInput.actions["Tap"];
        dragAction = playerInput.actions["Drag"];
        
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();

        tapAction.performed += TapPressed;
        dragAction.performed += DragPerformed;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += StopTouch;
    }

    private void OnDisable()
    {
        tapAction.performed -= TapPressed;
        dragAction.performed -= DragPerformed;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= StopTouch;
    }

    private void TapPressed(InputAction.CallbackContext context)
    {
        OnTap?.Invoke(this, Touchscreen.current.primaryTouch.position.ReadValue());
    }

    private void DragPerformed(InputAction.CallbackContext context)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        position.z = 0.0f;
        OnDrag?.Invoke(this, position);
    }

    private void StopTouch(Finger fin)
    {
        OnDragCancelled?.Invoke(this, EventArgs.Empty);
    }

}
