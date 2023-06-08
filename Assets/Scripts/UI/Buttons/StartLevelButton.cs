using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevelButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    public void OnTap()
    {
        Time.timeScale = 1;
    }
}
