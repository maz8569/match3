using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Match3ScriptableObjects/Recipe")]
[System.Serializable]
public class RecipeSO : ScriptableObject
{
    public string RecipeName;
    public Sprite Sprite;
    public Sprite ServedSprite;
    public ItemSO Ingredient1;
    public ItemSO Ingredient2;
    public ItemSO Ingredient3;
    public string Description;

    public List<ItemSO> GetItemList()
    {
        var list = new List<ItemSO>();

        if(Ingredient1 != null) list.Add(Ingredient1);
        if(Ingredient2 != null) list.Add(Ingredient2);
        if(Ingredient3 != null) list.Add(Ingredient3);

        return list;
    }
}
