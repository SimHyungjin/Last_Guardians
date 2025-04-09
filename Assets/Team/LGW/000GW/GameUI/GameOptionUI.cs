using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptionUI : MonoBehaviour
{
    public GameObject optionSlot;
    public GameObject optionPanel;
    public GameObject HomePanel;
    public void OpenOption()
    {
        optionSlot.SetActive(!optionSlot.activeSelf);
    }
    public void CloseOption()
    {
        optionSlot.SetActive(false);
    }

    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
        Time.timeScale = 1; 
    }
    public void OpenOptionPanel()
    {
        optionPanel.SetActive(true);
        optionSlot.SetActive(false);
        Time.timeScale = 0; 
    }

    public void CloseHomePanel()
    {
        HomePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void OpenHomePanel()
    {
        HomePanel.SetActive(true);
        optionSlot.SetActive(false);
        Time.timeScale = 0;
    }

}
