using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePageButton : MonoBehaviour
{
    [SerializeField] private bool _previous;

    [SerializeField] private CookBook _cookBook;

    private void ChangePage()
    {
        if(!_previous)
        {
            if(_cookBook.currentPage < _cookBook._recipes.Count - 1)
            {
                _cookBook.currentPage++;
            }
            else
            {
                _cookBook.currentPage = 0;
            }
                
        }
        else
        {
            if(_cookBook.currentPage > 0)
            {
                _cookBook.currentPage--;
            }
            else
            {
              _cookBook.currentPage = _cookBook._recipes.Count - 1;  
            }
        }

        _cookBook.LoadRecipe(_cookBook.currentPage);
    }

    public void OnTap()
    {
        AudioPlayer.Instance.Play(Clip.PAGE_FLIP);
        ChangePage();
    }
}
