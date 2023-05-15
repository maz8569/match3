using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Match3UI : MonoBehaviour
{
    [SerializeField] private Match3 match3;

    [SerializeField] private TextMeshProUGUI recipeText;
    [SerializeField] private LineRenderer lineRenderer;

    private void OnEnable()
    {
        match3.OnNewItemChanged += ItemChanged;
        match3.OnMove += Clear;
    }

    private void OnDisable()
    {
        match3.OnNewItemChanged -= ItemChanged;
        match3.OnMove -= Clear;
    }

    private void ItemChanged(object sender, System.EventArgs e)
    {
        if(match3.GetLastChosen() != null)
        {
            recipeText.text = match3.GetLastChosen().RecipeName;
        }
        else
        {
            recipeText.text = "???";
        }
        
        if(match3.GetLastChosenItemPosition() == new Vector2(-1, -1))
        {
            Debug.Log("NO");
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = match3.GetChosenItemsPositionCount();
        lineRenderer.SetPosition(match3.GetChosenItemsPositionCount() - 1, match3.GetLastChosenItemPosition());
    }

    private void Clear(object sender, System.EventArgs e)
    {
        lineRenderer.positionCount = 0;
        recipeText.text = "";
    }

}
