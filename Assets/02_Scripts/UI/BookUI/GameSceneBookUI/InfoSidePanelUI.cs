using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoSidePanelUI : MonoBehaviour
{
    public static InfoSidePanelUI Instance { get; private set; }

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private GameObject panelContainer;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        panelContainer.SetActive(false);
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(Hide);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void Show(TowerData data)
    {
        if (!panelContainer) return;

        icon.sprite = TowerIconContainer.Instance.GetSprite(data.TowerIndex);
        nameText.text = data.TowerName;
        descText.text = data.TowerDescription;
        panelContainer.SetActive(true);
    }

    public void Hide()
    {
        if (!panelContainer) return;
        panelContainer.SetActive(false);
    }
}
