using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void OnTap()
    {
        //SceneManager.LoadSceneAsync(ProgressManager.Instance.currentWeek * ProgressManager.Instance.currentDay); //TODO: move to ProgressManager
        SceneManager.LoadSceneAsync(1);
    }
}
