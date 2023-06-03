using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookBook : MonoBehaviour
{
    [SerializeField] public List<RecipeSO> _recipes; //TODO: privat with setters
    [SerializeField] private Image _dishImg;
    [SerializeField] private List<GameObject> _dishIngredients;
    [SerializeField] private TextMeshProUGUI _dishDescription;

    public int currentPage = 0;

    void Start(){
        LoadRecipe(0);
    }

    public void LoadRecipe(int recipeNr)
    {
        RecipeSO wantedRecipe = _recipes[recipeNr];

        _dishImg.sprite = wantedRecipe.Sprite;

        if(wantedRecipe.Ingredient1 != null)
        {
            _dishIngredients[0].transform.GetChild(1).GetChild(0).GetComponentInChildren<Image>().sprite = wantedRecipe.Ingredient1.Sprite; //TODO: list of img
            _dishIngredients[0].transform.GetComponentInChildren<TextMeshProUGUI>().text = wantedRecipe.Ingredient1.ItemName;
            _dishIngredients[0].SetActive(true);
        }
        if(wantedRecipe.Ingredient2 != null)
        {
            _dishIngredients[1].transform.GetChild(1).GetChild(0).GetComponentInChildren<Image>().sprite = wantedRecipe.Ingredient2.Sprite;
            _dishIngredients[1].transform.GetComponentInChildren<TextMeshProUGUI>().text = wantedRecipe.Ingredient2.ItemName;
            _dishIngredients[1].SetActive(true);
        }
        if(wantedRecipe.Ingredient3 != null)
        {
            _dishIngredients[2].transform.GetChild(1).GetChild(0).GetComponentInChildren<Image>().sprite = wantedRecipe.Ingredient3.Sprite;
            _dishIngredients[2].transform.GetComponentInChildren<TextMeshProUGUI>().text = wantedRecipe.Ingredient3.ItemName;
            _dishIngredients[2].SetActive(true);
        }

        //TODO: dish description
    }
}
