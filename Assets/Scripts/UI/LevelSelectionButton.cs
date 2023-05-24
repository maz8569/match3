using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionButton : MonoBehaviour
{
    [SerializeField] int levelNr;

    private void ChangeLevel()
    {
        SceneManager.LoadSceneAsync(levelNr);
    }

    public void OnTouch()
    {
        ChangeLevel();
    }
}
