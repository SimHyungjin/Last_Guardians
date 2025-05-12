using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeUI : MonoBehaviour
{
    [Header("������")]
    public TowerUpgrade towerUpgrade;
    [SerializeField] private TextMeshProUGUI currentMasteryPoint; 
    [Header("����â")]
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TextMeshProUGUI descriptionTitleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("���׷��̵� ��ư")]
    [SerializeField] private List<Button> buttons;
    [SerializeField] private Image fillBox;
    void Start()
    {
        currentMasteryPoint.text = towerUpgrade.towerUpgradeData.currentMasteryPoint.ToString();
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;  
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }
private void OnButtonClicked(int key)
    {

    }
}
