using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GiveUpButton : MonoBehaviour
{
    private void GiveUp()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void OnClick()
    {
        GiveUp();
        Time.timeScale = 1;
    }
}
