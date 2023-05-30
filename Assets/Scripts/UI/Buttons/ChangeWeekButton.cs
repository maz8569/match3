using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWeekButton : MonoBehaviour
{
    [SerializeField] private bool _previous;
    [SerializeField] private WeekMenu _weekMenu;

    [SerializeField] private List<Sprite> _arrowSprites;

    //TODO: change other arrow to active img after each ChangeWeek();

    private void ChangeWeek()
    {
        if(!_previous)
        {
            if(ProgressManager.Instance.currentWeek == ProgressManager.WEEKS - 1)
            {
                ProgressManager.Instance.currentWeek += 1;
                GetComponent<Image>().sprite = _arrowSprites[0];
            }
            else if(ProgressManager.Instance.currentWeek < ProgressManager.WEEKS)
            {
                ProgressManager.Instance.currentWeek += 1;
                GetComponent<Image>().sprite = _arrowSprites[1];
            }
            else
            {
                return;
            }
        }
        else
        {
            if(ProgressManager.Instance.currentWeek == 2)
            {
                ProgressManager.Instance.currentWeek -= 1;
                GetComponent<Image>().sprite = _arrowSprites[0];
            }
            else if(ProgressManager.Instance.currentWeek > 1)
            {
                ProgressManager.Instance.currentWeek -= 1;
                GetComponent<Image>().sprite = _arrowSprites[1];
            }
            else
            {
                return;
            }
        }
        _weekMenu.ChangeWeek();
    }

    public void OnTap()
    {
        ChangeWeek();
    }
}
