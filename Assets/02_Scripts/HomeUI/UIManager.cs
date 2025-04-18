using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("패널")]
    public GameObject optionPanel;
    public GameObject equipmentPanel;
    public GameObject bookPanel;

    [Header("UI 클릭 막는 블로커")]
    public GameObject interactionBlocker;

    private bool isProcessing = false;

    public void OpenOption()
    {
        if (isProcessing) return;
        isProcessing = true;

        CloseAllPanels();
        optionPanel.SetActive(true);
        interactionBlocker.SetActive(true);

        StartCoroutine(ResetProcessingFlag());
    }

    public void OpenEquipmentPanel()
    {
        if (isProcessing) return;
        isProcessing = true;

        CloseAllPanels();
        equipmentPanel.SetActive(true);
        interactionBlocker.SetActive(true);

        StartCoroutine(ResetProcessingFlag());
    }

    public void OpenBookPanel()
    {
        if (isProcessing) return;
        isProcessing = true;

        CloseAllPanels();
        bookPanel.SetActive(true);
        interactionBlocker.SetActive(true);

        StartCoroutine(ResetProcessingFlag());
    }

    public void CloseOption()
    {
        optionPanel.SetActive(false);
        interactionBlocker.SetActive(false);
    }

    public void CloseEquipmentPanel()
    {
        equipmentPanel.SetActive(false);
        interactionBlocker.SetActive(false);
    }

    public void CloseBookPanel()
    {
        bookPanel.SetActive(false);
        interactionBlocker.SetActive(false);
    }

    private void CloseAllPanels()
    {
        optionPanel.SetActive(false);
        equipmentPanel.SetActive(false);
        bookPanel.SetActive(false);
        interactionBlocker.SetActive(false);
    }

    private IEnumerator ResetProcessingFlag()
    {
        yield return new WaitForSeconds(0.2f);
        isProcessing = false;
    }
}



