using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TowerCodexUI : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform gridParent;
    public GameObject dummySpacerPrefab;

    public ScrollRect scrollRect;
    public List<Button> codexButtons = new List<Button>();

    public Button closeCombinationButton; 

    private void Start()
    {
        

        var allData = Resources.LoadAll<TowerData>("SO/Tower").ToList();
        allData = allData.OrderBy(t => t.TowerIndex).ToList();

        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        foreach (var data in allData)
        {
            var go = Instantiate(entryPrefab, gridParent);
            var ui = go.GetComponent<TowerEntryUI>();
            ui.SetData(data);
            codexButtons.Add(ui.entryButton); 
        }

        if (dummySpacerPrefab != null)
            Instantiate(dummySpacerPrefab, gridParent);
    }

    public void SetInteractable(bool active)
    {
        scrollRect.enabled = active;

        foreach (var btn in codexButtons)
        {
            btn.interactable = active;
        }

        
        if (closeCombinationButton != null)
            closeCombinationButton.interactable = true;
    }
}
