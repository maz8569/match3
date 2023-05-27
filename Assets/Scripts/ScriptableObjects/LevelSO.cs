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
    public int clientsNumber;
    public float cellSize;
    public float cellDistance;
    public List<RecipeSO> recipes;
    public List<LevelGridPosition> levelGridPositions;

    [System.Serializable]
    public class LevelGridPosition
    {
        public ItemSO itemSO;
        public int x;
        public int y;
    }
}
