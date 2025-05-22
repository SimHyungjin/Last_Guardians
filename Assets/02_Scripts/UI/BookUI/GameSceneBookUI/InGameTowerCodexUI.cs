using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameTowerCodexUI : MonoBehaviour
{
    public static InGameTowerCodexUI Instance { get; private set; }

    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject panelContainer;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        panelContainer.SetActive(false);
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(Hide);
    }

    public void Open()
    {
       
        for (int i = gridParent.childCount - 1; i >= 0; i--)
            Destroy(gridParent.GetChild(i).gameObject);

        
        List<TowerData> cards = MulliganUI.Instance.GetSelectedCards();
        if (cards == null || cards.Count == 0) return;

        foreach (var data in cards)
        {
            GameObject go = Instantiate(entryPrefab, gridParent);
            go.GetComponent<InGameTowerEntryUI>().Init(data);
        }

        panelContainer.SetActive(true);
        scrollRect.verticalNormalizedPosition = 1f;
    }

    public void Hide()
    {
        panelContainer.SetActive(false);
    }
}
