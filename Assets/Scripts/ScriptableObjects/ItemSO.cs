using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Match3ScriptableObjects/Item")]
[System.Serializable]
public class ItemSO : ScriptableObject
{
    public string ItemName;
    public Sprite Sprite;
}
