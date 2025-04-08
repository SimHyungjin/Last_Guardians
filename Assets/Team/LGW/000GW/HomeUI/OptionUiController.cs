using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUIController : MonoBehaviour
{
    public GameObject optionPanel;
    public GameObject equipmentPanel;

    public void OpenOption()
    {
        optionPanel.SetActive(true);
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
}
