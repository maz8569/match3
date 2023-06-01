using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    public RecipeSO desiredDish {get; private set;}
    
    //TODO: order variables
    public const float PATIENCE = 60;
    private float _waitingTime = 0;
    public int _seatNr {get; set;}
    public DiningHall diningHall;
    public float state = 0.0f;

    public Match3 _match3; //TODO: auto-fetch
    private LevelSO _currentLevel;
    [SerializeField] private Image _progressBar;
   
    private void Start()
    {
        _currentLevel = _match3.levelSO; //TODO: fetch from DiningHall

        var random = new System.Random();
        Random.InitState(System.DateTime.Now.Millisecond); //TODO: check if necessary for each client instance

        desiredDish = _currentLevel.recipes[random.Next(_currentLevel.recipes.Count)];

        StartCoroutine(StartCountdown());

    }

    public IEnumerator StartCountdown(float countdownValue = PATIENCE)
    {
        _waitingTime = countdownValue;
        while (_waitingTime >= 0)
        {
            state = _waitingTime / PATIENCE;

            _progressBar.fillAmount = state;

            if(state < 0.2f){ //TODO: magic numbers
                _progressBar.color = Color.red;
            } else if(state < 0.5f){ //TODO: magic numbers
                _progressBar.color = Color.yellow;
            } else{
                _progressBar.color = Color.green;
            }
            
            yield return new WaitForSeconds(0.1f);//TODO: magic numbers
            _waitingTime -= 0.1f; //TODO: magic numbers
        }
        diningHall.DeleteClient(this); //TODO: delegation/event
    }

}
