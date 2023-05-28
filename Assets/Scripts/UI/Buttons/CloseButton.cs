using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    private void Close()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void OnTap()
    {
        Close();
    }
}
