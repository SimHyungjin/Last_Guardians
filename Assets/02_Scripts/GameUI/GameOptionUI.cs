using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptionUI : MonoBehaviour
{
    public GameObject optionSlot;
    public GameObject optionPanel;
    public GameObject HomePanel;

    private bool isOptionPanelOpen = false;
    private bool isHomePanelOpen = false;
    private bool isOptionOpen = false;

    public void OpenOption()
    {
        isOptionOpen = !isOptionOpen;
        optionSlot.SetActive(isOptionOpen);
    }

    public void CloseOption()
    {
        optionSlot.SetActive(false);
    }

    public void OpenOptionPanel()
    {
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
        if (isHomePanelOpen) return;

        isHomePanelOpen = true;
        HomePanel.SetActive(true);
        optionSlot.SetActive(false);
        Time.timeScale = 0;
    }

    public void CloseHomePanel()
    {
        if (!isHomePanelOpen) return;

        isHomePanelOpen = false;
        HomePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
