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
        transform.parent.parent.parent.gameObject.SetActive(false); //TODO: serializefield (?)
    }

    public void OnTouch()
    {
        UnpauseTime();
        CloseMenu();
    }
}
