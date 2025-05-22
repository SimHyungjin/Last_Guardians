using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class InfoSidePanelUI : MonoBehaviour, IPointerClickHandler
{
    public static InfoSidePanelUI Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject blocker;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        panel.SetActive(false);
        blocker.SetActive(false);
    }

    public void Show(TowerData data)
    {
        icon.sprite = TowerIconContainer.Instance.GetSprite(data.TowerIndex);
        nameText.text = data.TowerName;
        descText.text = data.TowerDescription;

        blocker.SetActive(true);
        panel.SetActive(true);
    }

   
    public void Hide()
    {
        panel.SetActive(false);
        blocker.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
     
        Hide();
   
        InGameCombinationUI.Instance.Hide();
    }
}
