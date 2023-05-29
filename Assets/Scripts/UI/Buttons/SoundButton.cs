using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : StateButton
{
    private void TurnSoundOn()
    {

    }

    public void OnTap()
    {
        TurnSoundOn();
        ChangeImg();
    }
}
