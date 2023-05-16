using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesiredDish : MonoBehaviour
{
    private Camera _mainCamera;
    private GameObject _ingredientsList;

    [SerializeField]
    private Client _parent;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        //Set camera for RayCasting
        _mainCamera = Camera.main;
        _mainCamera.transparencySortMode = TransparencySortMode.Orthographic;
    }

    private void Start()
    {
        //Fetch necessary items
        _spriteRenderer.sprite = _parent.desiredDish.Sprite;
        _ingredientsList = transform.GetChild(0).gameObject;

        //Set ingredients sprites
        //TODO: ingredients in list(?)
        if(_parent.desiredDish.Ingredient1 != null)
        {
            _ingredientsList.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = _parent.desiredDish.Ingredient1.Sprite; //TODO: check for exceptions
        }
        if(_parent.desiredDish.Ingredient2 != null)
        {
            _ingredientsList.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = _parent.desiredDish.Ingredient2.Sprite; //TODO: check for exceptions
        }
        if(_parent.desiredDish.Ingredient3 != null)
        {
            _ingredientsList.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = _parent.desiredDish.Ingredient3.Sprite; //TODO: check for exceptions
        }
    }

    public void OnTap(InputAction.CallbackContext context) //TODO: one touch handler(?)
    {
        if(!context.started) return;

        Vector2 touchPosition;

        //Check platform before retrieving tap/touch position
        #if UNITY_ANDROID || UNITY_IOS
            touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        #elif UNITY_STANDALONE
            touchPosition = Mouse.current.position.ReadValue();
        #endif

        //Check if tapped/touched on an object
        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(touchPosition));
        if (!rayHit.collider) return;
        
        //Enable/disable ingredients list
        _ingredientsList.SetActive(!_ingredientsList.activeSelf);
        _spriteRenderer.enabled = !_spriteRenderer.enabled;

    }
    
}
