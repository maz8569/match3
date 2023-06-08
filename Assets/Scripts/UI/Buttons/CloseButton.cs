using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    [SerializeField] GameObject _shade;
    private void Close()
    {
        transform.parent.gameObject.SetActive(false);
        _shade?.SetActive(false);
    }

    public void OnTap()
    {
        Close();
    }
}
