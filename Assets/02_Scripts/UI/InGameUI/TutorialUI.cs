using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        }
        else
        {
            EndTutorial();
        }
    }

    private void PrePage()
    {
        if (isTransitioning) return;

        if (currentPage - 1 >= 0)
        {
            currentPage--;
            ShowPage(currentPage);
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

    private void EndTutorial()
    {
        if (this.gameObject.name == "InGameTutorial")
            PlayerPrefs.SetInt("InGameTutorial", 1);

        if (this.gameObject.name == "UpgradeTutorial")
        {
            PlayerPrefs.SetInt("UpgradeTutorial", 1);
            MainSceneManager.Instance.ShowPanel("TowerUpgrade");
        }


        if (this.gameObject.name == "EquipTutorial")
        {
            PlayerPrefs.SetInt("EquipTutorial", 1);
            MainSceneManager.Instance.ShowPanel("InventoryGroup");
        }


        gameObject.SetActive(false);
        if (GameManager.Instance.GetSceneName() == "GameScene")
            InGameManager.Instance.MuliigunStart();
    }
}
