using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Match3ScriptableObjects/Level")]
[System.Serializable]
public class LevelSO : ScriptableObject
{
    public List<ItemSO> items;
    public int width;
    public int height;
    public int targetScore;
    public List<RecipeSO> recipes;

}
