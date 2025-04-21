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
    public TowerCodexUI codexUI;
    private bool isProcessing = false;

    public void OpenOption()
    {
        if (isProcessing) return;
        StartCoroutine(HandlePanelOpen(optionPanel, false));
    }

    public void OpenEquipmentPanel()
    {
        if (isProcessing) return;
        StartCoroutine(HandlePanelOpen(equipmentPanel, true));
    }

    public void OpenBookPanel()
    {
        if (isProcessing) return;
        isProcessing = true;

        CloseAllPanels();
        bookPanel.SetActive(true);
        interactionBlocker.SetActive(true);

        if (codexUI != null)
        {
            codexUI.RefreshToAll(); 
        }

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

    private IEnumerator HandlePanelOpen(GameObject panelToOpen, bool useBlocker)
    {
        isProcessing = true;

        CloseAllPanels();
        panelToOpen.SetActive(true);
        interactionBlocker.SetActive(useBlocker);

        yield return new WaitForSeconds(0.2f);
        isProcessing = false;
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
