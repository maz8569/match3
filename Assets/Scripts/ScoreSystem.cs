using System.Collections;
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

    private float _starZoomDuration;
    private float _scaleModifier = 1;
    public float lerpDuration = 0.2f;
    public float targetScale = 1.2f;

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
                StartCoroutine(FillStar(stars[i]));
                stars[i].gameObject.GetComponent<Image>().sprite = filledStar;
            }
            else
            {
                stars[i].gameObject.GetComponent<Image>().sprite = unfilledStar;
            }
        }

        progressBarFill.fillAmount = e.progress;
    }

    public IEnumerator FillStar(RectTransform star)
    {
        Debug.Log(star.name);
        float time = 0;
        float startValue = _scaleModifier;
        Vector3 startScale = transform.localScale;

        while (time < _starZoomDuration)
        {
            _scaleModifier = Mathf.Lerp(startValue, targetScale, time / lerpDuration);
            star.localScale = startScale * _scaleModifier;
            time += Time.deltaTime;
            yield return null;
        }

        star.localScale = startScale * targetScale;
        _scaleModifier = targetScale;

    }

}
