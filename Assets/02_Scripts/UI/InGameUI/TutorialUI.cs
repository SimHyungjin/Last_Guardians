using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Button skipButton;
 
    private void Start()
    {
        skipButton.onClick.AddListener(EndTutorial);
    }

    private void EndTutorial()
    {

        if (this.gameObject.name == "UpgradeTutorial")
        {
            PlayerPrefs.SetInt("UpgradeTutorial", 1);
            MainSceneManager.Instance.ShowPanel("TowerUpgrade");
        }


        if (this.gameObject.name == "EquipTutorial")
        {
            PlayerPrefs.SetInt("EquipTutorial", 1);
            MainSceneManager.Instance.ShowPanel("InventoryGroup");
        }


        gameObject.SetActive(false);
       
    }
}
