using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultButton : MonoBehaviour
{
    protected void PlayPressedSound()
    {
        AudioPlayer.Instance.PlayAudio(Clip.BUTTON_PRESS);
    }
}
