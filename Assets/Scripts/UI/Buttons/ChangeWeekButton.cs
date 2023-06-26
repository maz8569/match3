using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWeekButton : DefaultButton
{
    [SerializeField] private bool _previous;
    [SerializeField] private WeekMenu _weekMenu;

    [SerializeField] private List<Sprite> _arrowSprites;
    [SerializeField] private ChangeWeekButton _otherArrow;

    //TODO: change other arrow to active img after each ChangeWeek();

    public void SetArrow(bool active)
    {
        GetComponent<Image>().sprite = active ? _arrowSprites[1] : _arrowSprites[0];
    }

    private void ChangeWeek()
    {
        if(!_previous)
        {
            if(ProgressManager.Instance.CurrentWeek == ProgressManager.WEEKS - 1)
            {
                ProgressManager.Instance.CurrentWeek += 1;
                SetArrow(false);
            }
            else if(ProgressManager.Instance.CurrentWeek < ProgressManager.WEEKS)
            {
                ProgressManager.Instance.CurrentWeek += 1;
                SetArrow(true);
            }
            else
            {
                return;
            }
        }
        else
        {
            if(ProgressManager.Instance.CurrentWeek == 2)
            {
                ProgressManager.Instance.CurrentWeek -= 1;
                SetArrow(false);
            }
            else if(ProgressManager.Instance.CurrentWeek > 1)
            {
                ProgressManager.Instance.CurrentWeek -= 1;
                SetArrow(true);
            }
            else
            {
                return;
            }
        }

        _otherArrow?.SetArrow(true);
        _weekMenu.ChangeWeek();
    }

    public void OnTap()
    {
        ChangeWeek();
    }
}
