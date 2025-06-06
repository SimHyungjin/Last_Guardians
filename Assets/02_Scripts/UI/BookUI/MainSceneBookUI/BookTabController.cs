using UnityEngine;

public class BookTabController : MonoBehaviour
{
    public GameObject towerBookPanel;
    public GameObject towerComboPanel;

    private void Start()
    {
        ShowTowerBook();
    }
    public void ShowTowerBook()
    {
        towerBookPanel.SetActive(true);
        towerComboPanel.SetActive(false);
    }
    public void ShowTowerCombo()
    {
        towerBookPanel.SetActive(false);
        towerComboPanel.SetActive(true);
    }
}
