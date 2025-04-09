using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject optionPanel;
    public GameObject equipmentPanel;
    public GameObject bookPanel;

    public void OpenOption()
    {
        optionPanel.SetActive(!optionPanel.activeSelf);
    }

    public void CloseOption()
    {
        optionPanel.SetActive(false);
    }

    public void CloseEquipmentPanel()
    {
        equipmentPanel.SetActive(false);
    }
    public void OpenEquipmentPanel()
    {
        equipmentPanel.SetActive(true);
    }

    public void OpenBookPanel()
    {
        bookPanel.SetActive(true);
    }
    public void CloseBookPanel()
    {
        bookPanel.SetActive(false);
    }
}
