using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelNr;
    [SerializeField] private Match3 _match3;

    private void Start()
    {
        _levelNr.text = _match3.levelSO.name;
    }
}
