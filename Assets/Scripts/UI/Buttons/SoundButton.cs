using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : StateButton
{
    private void Awake()
    {
        ChangeImg(!AudioPlayer.Instance.audioMuted);
    }

    private void TurnSoundOn()
    {
        AudioPlayer.Instance.audioMuted = !AudioPlayer.Instance.audioMuted;
    }

    public void OnTap()
    {
        TurnSoundOn();
        PlayPressedSound();
        ChangeImg();
    }
}
