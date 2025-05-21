using UnityEngine;
using UnityEngine.UI;

public class GameOptionUI : MonoBehaviour
{
    public GameObject optionSlot;
    public GameObject optionPanel;
    public GameObject homePanel;
    public GameObject bookPanel;
    public GameObject tutorialPanel;
    public Button gameSpeedButton;
    public Sprite[] gameSpeedButtonImages;
    private int gameSpeedIndex = 0;
    private bool isOptionPanelOpen;
    private bool isHomePanelOpen;
    private bool isBookPanelOpen;
    private bool isTutoOpen;

    public void OpenBook()
    {
        SoundManager.Instance.PlaySFX("PopUp");
        if (isBookPanelOpen) return;
        isBookPanelOpen = true;
        bookPanel.SetActive(true);
        optionSlot.SetActive(false);
        TowerManager.Instance.hand.gameObject.SetActive(false);
        Time.timeScale = 0;
    }
    public void CloseBook()
    {
        if (!isBookPanelOpen) return;
        isBookPanelOpen = false;
        bookPanel.SetActive(false);
        TowerManager.Instance.hand.gameObject.SetActive(true);
        Time.timeScale = InGameManager.Instance.TimeScale;
    }

    public void ChangeGameSpeed()
    {
        InGameManager.Instance.SetTimeScale();
        gameSpeedIndex += 1;
        if (gameSpeedIndex >= gameSpeedButtonImages.Length)
        {
            gameSpeedIndex = 0;
        }
        gameSpeedButton.GetComponent<Image>().sprite = gameSpeedButtonImages[gameSpeedIndex];

    }

    public void OpenOption()
    {
        bool next = !optionSlot.activeSelf;
        optionSlot.SetActive(next);
    }

    public void OpenOptionPanel()
    {
        SoundManager.Instance.PlaySFX("PopUp");
        if (isOptionPanelOpen) return;
        isOptionPanelOpen = true;
        optionPanel.SetActive(true);
        optionSlot.SetActive(false);
        TowerManager.Instance.hand.gameObject.SetActive(false);
        Time.timeScale = 0;
    }
    public void CloseOptionPanel()
    {
        if (!isOptionPanelOpen) return;
        isOptionPanelOpen = false;
        optionPanel.SetActive(false);
        TowerManager.Instance.hand.gameObject.SetActive(true);
        Time.timeScale = InGameManager.Instance.TimeScale;
    }

    public void OpenHomePanel()
    {
        SoundManager.Instance.PlaySFX("PopUp");
        if (isHomePanelOpen) return;
        isHomePanelOpen = true;
        homePanel.SetActive(true);
        optionSlot.SetActive(false);
        TowerManager.Instance.hand.gameObject.SetActive(false);
        Time.timeScale = 0;
    }
    public void CloseHomePanel()
    {
        if (!isHomePanelOpen) return;
        isHomePanelOpen = false;
        homePanel.SetActive(false);
        TowerManager.Instance.hand.gameObject.SetActive(true);
        Time.timeScale = InGameManager.Instance.TimeScale;
    }

    public void OpenTutorial()
    {
        SoundManager.Instance.PlaySFX("PopUp");
        if (tutorialPanel.gameObject.activeSelf) return;
        InGameManager.Instance.isTutorial = false;
        isTutoOpen = true;
        tutorialPanel.SetActive(true);
        optionSlot.SetActive(false);
        TowerManager.Instance.hand.gameObject.SetActive(false);
        Time.timeScale = 0;
    }
}
