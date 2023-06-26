using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : DefaultButton
{
    public void OnTap()
    {
        PlayPressedSound();
        SceneManager.LoadSceneAsync(1);
    }
}
