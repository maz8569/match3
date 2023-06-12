using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideButton : MonoBehaviour
{
    [SerializeField] private GameObject _thisVisual;
    [SerializeField] private GameObject _otherVisual;

    private void Hide()
    {
        _otherVisual?.SetActive(true);
        _thisVisual.SetActive(false);
    }

    public void OnTap()
    {
        Hide();
    }
}
