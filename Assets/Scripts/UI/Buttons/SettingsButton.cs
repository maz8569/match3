using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenu; //TODO: spawn menu as prefab(?)

    public void OnTap()
    {
        _settingsMenu.SetActive(true);
    }
}
