using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButton : StateButton
{
    private void TurnMusicOn()
    {

    }

    public void OnTap()
    {
        TurnMusicOn();
        ChangeImg();
    }
}
