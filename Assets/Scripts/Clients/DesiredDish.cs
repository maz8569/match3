using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesiredDish : MonoBehaviour
{
    private Camera _mainCamera;
    private GameObject _ingredientsList;
    private TouchManager _touchManager;

    [SerializeField]
    private Client _parent;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        //Set camera for RayCasting
        _mainCamera = Camera.main;
        _mainCamera.transparencySortMode = TransparencySortMode.Orthographic;
        _touchManager = GameObject.Find("TouchManager").GetComponent<TouchManager>();
        _touchManager.OnTap += OnTap;
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

    public void OnTap(object sender, Vector3 touchPosition ) //TODO: one touch handler(?)
    {
        //Check if tapped/touched on an object
        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(touchPosition));
        if (!rayHit.collider) return;
        
        if(rayHit.collider == transform.GetComponent<BoxCollider2D>())
        {
            //Enable/disable ingredients list
            _ingredientsList.SetActive(!_ingredientsList.activeSelf);
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
        }

    }

    private void OnDestroy()
    {
        _touchManager.OnTap -= OnTap;
    }

}
