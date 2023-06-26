using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GiveUpButton : DefaultButton
{
    private void GiveUp()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void OnClick()
    {
        PlayPressedSound();
        GiveUp();
        Time.timeScale = 1;
    }
}
