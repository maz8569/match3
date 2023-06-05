using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public const int WEEKS = 5;
    public const int WEEKDAYS = 5;

    public int[,] results = new int[WEEKS, WEEKDAYS];
    public event EventHandler OnWeekChanged;
    [SerializeField]private int currentWeek = 1;

    public int CurrentWeek { get { return currentWeek; } set { 
            currentWeek = value; 
            OnWeekChanged?.Invoke(this, EventArgs.Empty); 
            DataPersistenceManager.instance.PushLoaded(); } }

    public int currentDay = 1;

    public LevelSO currentLevelScriptableObj;
    [SerializeField] private List<LevelSO> levelScriptableObjs = new List<LevelSO>();

    public void ChooseLevelScriptableObj()
    {
        if (levelScriptableObjs.Count >= currentDay)
        {
            currentLevelScriptableObj = levelScriptableObjs[currentDay - 1];
        }
    }

    public static ProgressManager Instance { get; private set; }
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 

        DontDestroyOnLoad(this.gameObject);
    }
}
