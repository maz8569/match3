using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class LevelData
{
    public SerializableDictionary<int, int> checkedStars;

    public LevelData()
    {
        checkedStars = new SerializableDictionary<int, int>();
    }
}
