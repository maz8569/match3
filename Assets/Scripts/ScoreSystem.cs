using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private Match3 match3;
    [SerializeField] private Image progressBarFill;
    [SerializeField] private List<RectTransform> stars;
    [SerializeField] private Sprite filledStar;
    [SerializeField] private Sprite unfilledStar;

    private void Start()
    {
        float width = progressBarFill.gameObject.GetComponent<RectTransform>().sizeDelta.x - stars[0].sizeDelta.x * 0.5f;
        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].anchoredPosition += new Vector2(match3.GetLevelSO().starsScore[i] * width, 0);
        }
    }

    private void OnEnable()
    {
        match3.OnScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        match3.OnScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(object sender, Match3.OnScoreChangedEventArgs e)
    {
        Debug.Log(e.progress);

        for (int i = 0; i < stars.Count; i++)
        {
            if(i < e.checkedStars)
            {
                stars[i].gameObject.GetComponent<Image>().sprite = filledStar;
            }
            else
            {
                stars[i].gameObject.GetComponent<Image>().sprite = unfilledStar;
            }
        }

        progressBarFill.fillAmount = e.progress;
    }

}
