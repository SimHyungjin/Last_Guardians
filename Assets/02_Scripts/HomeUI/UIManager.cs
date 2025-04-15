using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject optionPanel;
    public GameObject equipmentPanel;
    public GameObject bookPanel;

    private bool isProcessing = false;

    public void OpenOption()
    {
        if (isProcessing) return;
        CloseAllPanels(); 
        optionPanel.SetActive(true);
        StartCoroutine(ResetProcessingFlag());
    }

    public void OpenEquipmentPanel()
    {
        if (isProcessing) return;
        CloseAllPanels();
        equipmentPanel.SetActive(true);
        StartCoroutine(ResetProcessingFlag());
    }

    public void OpenBookPanel()
    {
        if (isProcessing) return;
        CloseAllPanels();
        bookPanel.SetActive(true);
        StartCoroutine(ResetProcessingFlag());
    }

    public void CloseOption() => optionPanel.SetActive(false);
    public void CloseEquipmentPanel() => equipmentPanel.SetActive(false);
    public void CloseBookPanel() => bookPanel.SetActive(false);

    private void CloseAllPanels()
    {
        optionPanel.SetActive(false);
        equipmentPanel.SetActive(false);
        bookPanel.SetActive(false);
    }

    private IEnumerator ResetProcessingFlag()
    {
        isProcessing = true;
        yield return new WaitForSeconds(0.2f); 
        isProcessing = false;
    }
}


