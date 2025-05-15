using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Image tutorialImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button preButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private CanvasGroup canvasGroup;

    [System.Serializable]
    public struct TutorialPage
    {
        public Sprite image;
        public string description;
    }

    [SerializeField] private TutorialPage[] pages;

    private int currentPage = 0;
    private bool isTransitioning = false;

    private void Start()
    {
        skipButton.onClick.AddListener(EndTutorial);
        nextButton.onClick.AddListener(NextPage);
        preButton.onClick.AddListener(PrePage);
        ShowPage(currentPage);
    }

    private void NextPage()
    {
        if (isTransitioning) return;

        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
            //StartCoroutine(FadePage());
        }
        else
        {
            EndTutorial();
        }
    }

    private void PrePage()
    {
        if (isTransitioning) return;

        if (currentPage-1 >= 0)
        {
            currentPage--;
            ShowPage(currentPage);
            //StartCoroutine(FadePage());
        }
    }

    private void ShowPage(int index)
    {
        tutorialImage.sprite = pages[index].image;
        descriptionText.text = pages[index].description;

        if (index == pages.Length - 1)
        {
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "시작하기";
        }
        else
        {
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "다음";
        }
    }

    private IEnumerator FadePage()
    {
        isTransitioning = true;

        // 페이드 아웃
        for (float t = 0; t < 1f; t += Time.deltaTime * 2f)
        {
            canvasGroup.alpha = 1f - t;
            yield return null;
        }
        canvasGroup.alpha = 0f;

        ShowPage(currentPage);

        // 페이드 인
        for (float t = 0; t < 1f; t += Time.deltaTime * 2f)
        {
            canvasGroup.alpha = t;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        isTransitioning = false;
    }

    private void EndTutorial()
    {
        //PlayerPrefs.SetInt("TutorialDone", 1);
        gameObject.SetActive(false);
        InGameManager.Instance.MuliigunStart();
    }
}
