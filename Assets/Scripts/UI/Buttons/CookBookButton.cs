using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookBookButton : DefaultButton
{
    [SerializeField] private GameObject _cookBook;

    public void OnTap()
    {
        PlayPressedSound();
        _cookBook.SetActiveRecursively(true);
    }
}
