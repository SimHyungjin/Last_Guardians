using UnityEngine;

public class GameOptionUI : MonoBehaviour
{
    public GameObject optionSlot;
    public GameObject optionPanel;
    public GameObject homePanel;
    public GameObject bookPanel;

    private bool isOptionPanelOpen;
    private bool isHomePanelOpen;
    private bool isBookPanelOpen;

    public void OpenBook()
    {
        SoundManager.Instance.PlaySFX("PopUp");
        if (isBookPanelOpen) return;
        isBookPanelOpen = true;
        bookPanel.SetActive(true);
        optionSlot.SetActive(false);
        Time.timeScale = 0;
    }
    public void CloseBook()
    {
        if (!isBookPanelOpen) return;
        isBookPanelOpen = false;
        bookPanel.SetActive(false);
        Time.timeScale = 1;
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
        Time.timeScale = 0;
    }
    public void CloseOptionPanel()
    {
        if (!isOptionPanelOpen) return;
        isOptionPanelOpen = false;
        optionPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenHomePanel()
    {
        SoundManager.Instance.PlaySFX("PopUp");
        if (isHomePanelOpen) return;
        isHomePanelOpen = true;
        homePanel.SetActive(true);
        optionSlot.SetActive(false);
        Time.timeScale = 0;
    }
    public void CloseHomePanel()
    {
        if (!isHomePanelOpen) return;
        isHomePanelOpen = false;
        homePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
