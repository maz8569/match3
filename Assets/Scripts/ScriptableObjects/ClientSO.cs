using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Match3ScriptableObjects/Client")]
[System.Serializable]
public class ClientSO : ScriptableObject
{
    public Sprite[] clientSprites;
}
