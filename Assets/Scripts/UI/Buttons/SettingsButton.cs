using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : DefaultButton
{
    [SerializeField] private GameObject _settingsMenu; //TODO: spawn menu as prefab(?)

    public void OnTap()
    {
        PlayPressedSound();
        _settingsMenu.SetActiveRecursively(true);
    }
}
