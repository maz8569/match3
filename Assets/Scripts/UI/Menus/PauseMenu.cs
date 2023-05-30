using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelNr;

    private void Start()
    {
        _levelNr.text = "Level " + SceneManager.GetActiveScene().buildIndex.ToString();
    }
}
