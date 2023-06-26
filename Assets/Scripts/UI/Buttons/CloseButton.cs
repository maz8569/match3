using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : DefaultButton
{
    [SerializeField] private GameObject _shade;
    [SerializeField] private GameObject _view;
    private void Close()
    {
        if(_view != null)
        {
            _view.SetActive(false);
        }
        else
        {
            transform.parent.gameObject.SetActive(false);
        }
        
        if(_shade != null)
        {
            _shade?.SetActive(false);
        }

    }

    public void OnTap()
    {
        PlayPressedSound();
        Time.timeScale = 1;
        Close();
    }
}
