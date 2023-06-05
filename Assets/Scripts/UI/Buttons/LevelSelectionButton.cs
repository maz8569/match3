using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour, IDataPesristence
{
    [SerializeField] private int dayNr;
    [SerializeField] private int levelNr;
    [SerializeField] private GameObject _stars;

    public List<Sprite> starsImg;

    private void Start()
    {
        ProgressManager.Instance.OnWeekChanged += OnWeekChanged;
    }

    private void OnWeekChanged(object sender, System.EventArgs e)
    {
        levelNr = ProgressManager.WEEKDAYS * (ProgressManager.Instance.CurrentWeek - 1) + dayNr;
    }

    private void OnDisable()
    {
        ProgressManager.Instance.OnWeekChanged -= OnWeekChanged;
    }

    private void ChangeLevel()
    {
        ProgressManager.Instance.currentDay = levelNr;
        ProgressManager.Instance.ChooseLevelScriptableObj();
        SceneManager.LoadSceneAsync(1); //TODO: move to ProgressManager
    }

    public void SetStars(int stars)
    {
        Debug.Log($"day nr {levelNr}");

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

    public void LoadData(LevelData levelData)
    {
        if (levelData.checkedStars.ContainsKey(levelNr))
        {
            SetStars(levelData.checkedStars[levelNr]);
            GetComponent<Button>().interactable = true;
        }
        else if(levelData.checkedStars.ContainsKey(levelNr - 1))
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            SetStars(0);
            GetComponent<Button>().interactable = false;
        }
    }

    public void SaveData(ref LevelData levelData)
    {
    }
}
