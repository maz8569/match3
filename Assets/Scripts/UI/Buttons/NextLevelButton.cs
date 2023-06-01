using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    private void LoadNextLevel()
    {
        //TODO: ProgressManager.LoadLevel()
    }

    public void OnTap()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
