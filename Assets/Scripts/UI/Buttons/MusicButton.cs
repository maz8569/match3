using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButton : StateButton
{
    private void TurnMusicOn()
    {
        AudioPlayer.Instance.MuteMusic(!AudioPlayer.Instance.musicMuted);
    }

    public void OnTap()
    {
        TurnMusicOn();
        PlayPressedSound();
        ChangeImg();
    }
}
