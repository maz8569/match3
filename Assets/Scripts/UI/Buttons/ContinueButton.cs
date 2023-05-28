using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    private void UnpauseTime()
    {
        Time.timeScale = 1;
    }

    private void CloseMenu()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void OnTouch()
    {
        UnpauseTime();
        CloseMenu();
    }
}
