using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SummaryScreen : MonoBehaviour
{
    [SerializeField] private List<GameObject> _dishes;
    [SerializeField] private Match3 _match3;
    [SerializeField] private DiningHall _diningHall;

    private LevelSO _levelSO;

    public void Activate()
    {   
        _levelSO = _match3.levelSO;
        for (int i = 0; i < _match3.levelSO.recipes.Count; i++)
        {
            var tmpDish = _dishes[i];
            tmpDish.SetActiveRecursively(true);

            tmpDish.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _diningHall._dishesSummary[_levelSO.recipes[i]].ToString();
            tmpDish.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = _levelSO.recipes[i].ServedSprite;
            
            if (_levelSO.recipes[i].Ingredient1 != null)
            {
                tmpDish.transform.GetChild(4).GetChild(0).GetComponent<Image>().sprite = _levelSO.recipes[i].Ingredient1.Sprite;
                tmpDish.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = _diningHall._recipesSummary[_levelSO.recipes[i]][_levelSO.recipes[i].Ingredient1].ToString();
            }
            if (_levelSO.recipes[i].Ingredient2 != null)
            {
                tmpDish.transform.GetChild(7).GetChild(0).GetComponent<Image>().sprite = _levelSO.recipes[i].Ingredient2.Sprite;
                tmpDish.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = _diningHall._recipesSummary[_levelSO.recipes[i]][_levelSO.recipes[i].Ingredient2].ToString();
            }
        }
    }
}
