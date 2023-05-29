using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideButton : MonoBehaviour
{
    [SerializeField] private GameObject _otherVisual;

    private void Hide()
    {
        _otherVisual?.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }

    public void OnTap()
    {
        Hide();
    }
}
