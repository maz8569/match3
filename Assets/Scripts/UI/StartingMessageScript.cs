using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartingMessageScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelNumber;

    private void Awake()
    {
        _levelNumber.text = "Level " + (ProgressManager.Instance.CurrentWeek * ProgressManager.Instance.currentDay);
    }
}
