using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : DefaultButton
{
    private void LoadNextLevel()
    {
        //TODO: ProgressManager.LoadLevel()
    }

    public void OnTap()
    {
        PlayPressedSound();
        SceneManager.LoadSceneAsync(0);
    }
}
