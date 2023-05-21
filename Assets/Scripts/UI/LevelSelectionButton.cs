using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionButton : MonoBehaviour
{
    [SerializeField] int levelNr;

    public void ChangeLevel()
    {
        SceneManager.LoadSceneAsync(levelNr);
    }
}
