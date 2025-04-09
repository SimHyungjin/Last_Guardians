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
    }
    public void OpenOptionPanel()
    {
        optionPanel.SetActive(true);
        optionSlot.SetActive(false);
    }

    public void CloseHomePanel()
    {
        HomePanel.SetActive(false);
    }
    public void OpenHomePanel()
    {
        HomePanel.SetActive(true);
        optionSlot.SetActive(false);
    }

}
