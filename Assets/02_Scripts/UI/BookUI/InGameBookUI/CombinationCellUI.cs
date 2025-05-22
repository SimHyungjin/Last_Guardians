using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombinationCellUI : MonoBehaviour
{
    [SerializeField] private Image iconA;
    [SerializeField] private Image iconB;
    [SerializeField] private Image iconResult;
    [SerializeField] private TextMeshProUGUI nameA;
    [SerializeField] private TextMeshProUGUI nameB;
    [SerializeField] private TextMeshProUGUI nameResult;

    
    [SerializeField] private Button resultButton;

    private TowerData resultData;

    public void Init(TowerData a, TowerData b, TowerData res)
    {
       
        iconA.sprite = TowerIconContainer.Instance.GetSprite(a.TowerIndex);
        iconB.sprite = TowerIconContainer.Instance.GetSprite(b.TowerIndex);
        iconResult.sprite = TowerIconContainer.Instance.GetSprite(res.TowerIndex);

        nameA.text = a.TowerName;
        nameB.text = b.TowerName;
        nameResult.text = res.TowerName;

        resultData = res;

        
        resultButton.onClick.RemoveAllListeners();
        resultButton.onClick.AddListener(OnClickResult);
    }

    private void OnClickResult()
    {
        InfoSidePanelUI.Instance.Show(resultData);
    }
}
