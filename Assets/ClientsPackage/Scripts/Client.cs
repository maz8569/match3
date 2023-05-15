using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    public RecipeSO _desiredDish {get; private set;}
    
    [SerializeField]
    private LevelSO _currentLevel; //TODO: fetch from Match3 GameObject
   
    private void Start()
    {
        var random = new System.Random();
        Random.InitState(System.DateTime.Now.Millisecond); //TODO: check if necessary for each client instance

        _desiredDish = _currentLevel.recipes[random.Next(_currentLevel.recipes.Count)];

        Debug.Log(_desiredDish);
    }

}
