using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    public RecipeSO desiredDish {get; private set;}
    
    //Client's patience in seconds
    public const float PATIENCE = 10;
    private float _waitingTime = 0;
    
    [SerializeField] private LevelSO _currentLevel; //TODO: fetch from Match3 GameObject
    [SerializeField] private Image _progressBar;
   
    private void Start()
    {
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
            float state = _waitingTime / PATIENCE;

            Debug.Log(state);

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
    }

}
