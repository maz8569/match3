using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateButton : MonoBehaviour //TODO: better name
{
    [SerializeField] protected Sprite _buttonOn;
    [SerializeField] protected Sprite _buttonOff;
    [SerializeField] protected Image _buttonImg;

    protected void ChangeImg()
    {
        if(_buttonImg.sprite == _buttonOff)
        {
            _buttonImg.sprite = _buttonOn;
        }
        else
        {
            _buttonImg.sprite = _buttonOff;
        }
    }
}
