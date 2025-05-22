// GameOptionUI.cs
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

    private void Awake()
    {
        // 필요 시 버튼 리스너 초기화
    }

    public void OpenBook()
    {
        SoundManager.Instance.PlaySFX("PopUp");
        if (isBookPanelOpen) return;
        isBookPanelOpen = true;

        optionSlot.SetActive(false);
        TowerManager.Instance.hand.gameObject.SetActive(false);
        Time.timeScale = 0;

        bookPanel.SetActive(true);
        InGameTowerCodexUI.Instance.Open();
    }

    public void CloseBook()
    {
        if (!isBookPanelOpen) return;
        isBookPanelOpen = false;

        InGameTowerCodexUI.Instance.Hide();
        bookPanel.SetActive(false);

        TowerManager.Instance.hand.gameObject.SetActive(true);
        TowerManager.Instance.EndInteraction(InteractionState.Pause);
        Time.timeScale = InGameManager.Instance.TimeScale;
    }

    public void ChangeGameSpeed()
    {
        InGameManager.Instance.SetTimeScale();
        gameSpeedIndex = (gameSpeedIndex + 1) % gameSpeedButtonImages.Length;
        gameSpeedButton.GetComponent<Image>().sprite = gameSpeedButtonImages[gameSpeedIndex];
    }

    public void OpenOption()
    {
        bool next = !optionSlot.activeSelf;
        SoundManager.Instance.PlaySFX("PopUp");
        optionSlot.SetActive(next);

        if (next)
        {
            Time.timeScale = 0;
            TowerManager.Instance.StartInteraction(InteractionState.Pause);
        }
        else
        {
            Time.timeScale = InGameManager.Instance.TimeScale;
            TowerManager.Instance.EndInteraction(InteractionState.Pause);
        }
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
        TowerManager.Instance.EndInteraction(InteractionState.Pause);
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
        TowerManager.Instance.EndInteraction(InteractionState.Pause);
        Time.timeScale = InGameManager.Instance.TimeScale;
    }

    public void OpenTutorial()
    {
        SoundManager.Instance.PlaySFX("PopUp");
        if (isTutoOpen) return;
        isTutoOpen = true;

        tutorialPanel.SetActive(true);
        optionSlot.SetActive(false);
        TowerManager.Instance.hand.gameObject.SetActive(false);
        TowerManager.Instance.EndInteraction(InteractionState.Pause);
        Time.timeScale = 0;
    }
}
