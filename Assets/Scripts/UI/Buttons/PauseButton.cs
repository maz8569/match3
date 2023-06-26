using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : DefaultButton
{
    [SerializeField] private GameObject _pauseMenu;

    private void PauseTime()
    {
        Time.timeScale = 0;
    }

    public void OnTouch()
    {
        PlayPressedSound();
        _pauseMenu.SetActive(true);
        PauseTime();
    }
}
