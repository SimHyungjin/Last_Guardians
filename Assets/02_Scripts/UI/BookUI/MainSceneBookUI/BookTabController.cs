using UnityEngine;
using UnityEngine.UI;

public class BookTabController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject towerBookPanel;
    public GameObject towerComboPanel;

    [Header("Tab Buttons")]
    [SerializeField] private Button bookTabButton;
    [SerializeField] private Button comboTabButton;

    [Header("Tab Colors")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;

    private void Start()
    {
        ShowTowerBook();
    }

    public void ShowTowerBook()
    {
     
        towerBookPanel.SetActive(true);
        towerComboPanel.SetActive(false);

     
        HighlightTab(bookTabButton);
    }

    public void ShowTowerCombo()
    {
        towerBookPanel.SetActive(false);
        towerComboPanel.SetActive(true);

        HighlightTab(comboTabButton);
    }

    private void HighlightTab(Button selected)
    {
      
        bookTabButton.image.color = defaultColor;
        comboTabButton.image.color = defaultColor;

       
        selected.image.color = selectedColor;
    }
}
