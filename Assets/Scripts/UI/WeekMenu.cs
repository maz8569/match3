using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeekMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _weekNr;
    [SerializeField] private GameObject _dayButtons;
    public void ChangeWeek()
    {
        _weekNr.text = "Week " + ProgressManager.Instance.currentWeek;

        foreach(Transform day in _dayButtons.transform)
        {
            day.GetComponent<LevelSelectionButton>().SetStars(ProgressManager.Instance.results[ProgressManager.Instance.currentWeek - 1, day.GetSiblingIndex()]);
        }
    }
}
