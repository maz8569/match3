using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
    [SerializeField] private int levelNr;
    [SerializeField] private GameObject _stars;

    public List<Sprite> starsImg;

    private void ChangeLevel()
    {
        SceneManager.LoadSceneAsync(levelNr * ProgressManager.Instance.currentWeek);
    }

    public void SetStars(int stars)
    {
        if (stars > 0)
        {
            for (int i = 0; i < stars; i++)
            {
                _stars.transform.GetChild(i).GetComponent<Image>().sprite = starsImg[2];
            }
            for (int i = stars; i < 3; i++) //TODO: magic number
            {
                _stars.transform.GetChild(i).GetComponent<Image>().sprite = starsImg[1];
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                _stars.transform.GetChild(i).GetComponent<Image>().sprite = starsImg[0];
            }
        }

    }

    public void OnTouch()
    {
        ChangeLevel();
    }
}
