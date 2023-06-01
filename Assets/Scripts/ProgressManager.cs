using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public const int WEEKS = 5;
    public const int WEEKDAYS = 5;

    public int[,] results = new int[WEEKS, WEEKDAYS];

    public int currentWeek = 1;
    public int currentDay = 1;

    public static ProgressManager Instance { get; private set; }
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 

        DontDestroyOnLoad(this.gameObject);

        results[2, 0] = 2;
    }
}
