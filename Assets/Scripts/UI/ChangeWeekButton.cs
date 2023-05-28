using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWeekButton : MonoBehaviour
{
    [SerializeField] private bool _previous;
    [SerializeField] private WeekMenu _weekMenu;

    [SerializeField] private List<Sprite> _arrowSprites;

    private void ChangeWeek()
    {
        if(!_previous)
        {
            if(ProgressManager.Instance.currentWeek < ProgressManager.WEEKS)
            {
                ProgressManager.Instance.currentWeek += 1;
                GetComponent<Image>().sprite = _arrowSprites[1];
            }
            else
            {
                GetComponent<Image>().sprite = _arrowSprites[0];
                return;
            }
        }
        else
        {
            if(ProgressManager.Instance.currentWeek > 1)
            {
                ProgressManager.Instance.currentWeek -= 1;
                GetComponent<Image>().sprite = _arrowSprites[1];
            }
            else
            {
                GetComponent<Image>().sprite = _arrowSprites[0];
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
