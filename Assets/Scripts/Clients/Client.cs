using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    public RecipeSO desiredDish {get; private set;}
    
    //TODO: order variables
    public const float PATIENCE = 20;
    private float _waitingTime = 0;
    public int _seatNr {get; set;}
    public DiningHall diningHall;
    public float state = 0.0f;

    public Match3 _match3; //TODO: auto-fetch
    private LevelSO _currentLevel;

    public GameObject plate;
    public GameObject cloud;
    public GameObject dish;

    private bool _served = false;

    private void Start()
    {
        _currentLevel = _match3.levelSO; //TODO: fetch from DiningHall

        var random = new System.Random();
        Random.InitState(System.DateTime.Now.Millisecond); //TODO: check if necessary for each client instance

        desiredDish = _currentLevel.recipes[random.Next(_currentLevel.recipes.Count)];

        InitializePlate();

        StartCoroutine(StartCountdown());

    }

    private void InitializePlate()
    {
        if(desiredDish.Ingredient1 != null)
        {
            plate.transform.GetChild(0).GetComponent<Image>().sprite = desiredDish.Ingredient1.Sprite; //TODO: check for exceptions
        }
        if(desiredDish.Ingredient2 != null)
        {
            plate.transform.GetChild(1).GetComponent<Image>().sprite = desiredDish.Ingredient2.Sprite; //TODO: check for exceptions
        }
        if(desiredDish.Ingredient3 != null)
        {
            plate.transform.GetChild(2).gameObject.SetActive(true);
            plate.transform.GetChild(2).GetComponent<Image>().sprite = desiredDish.Ingredient3.Sprite; //TODO: check for exceptions
        }
        else
        {
            plate.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public IEnumerator StartCountdown(float countdownValue = PATIENCE)
    {
        _waitingTime = countdownValue;
        while (_waitingTime >= 0)
        {
            state = _waitingTime / PATIENCE;

            if(_served){
                GetComponent<SpriteRenderer>().color = Color.blue;
            } else if(state < 0.01f){ //TODO: magic numbers
                GetComponent<SpriteRenderer>().color = Color.black;
            } else if(state < 0.2f){ //TODO: magic numbers
                GetComponent<SpriteRenderer>().color = Color.red;
            } else if(state < 0.5f){ //TODO: magic numbers
                GetComponent<SpriteRenderer>().color = Color.yellow;
            } else{
                GetComponent<SpriteRenderer>().color = Color.green;
            }
            
            yield return new WaitForSeconds(0.1f);//TODO: magic numbers
            _waitingTime -= 0.1f; //TODO: magic numbers
        }
        StartCoroutine(diningHall.DeleteClient(this)); //TODO: delegation/event
    }

    public void SetServed(bool value)
    {
        _served = value;
    }

    public bool IsServed()
    {
        return _served;
    }

}
