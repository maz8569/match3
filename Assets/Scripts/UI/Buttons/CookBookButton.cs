using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookBookButton : MonoBehaviour
{
    [SerializeField] private GameObject _cookBook;

    public void OnTap()
    {
        _cookBook.SetActiveRecursively(true);
    }
}
