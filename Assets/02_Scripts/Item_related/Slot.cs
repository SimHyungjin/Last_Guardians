using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 슬롯을 관리하는 클래스입니다.
/// </summary>
public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Image selectedEffect;
    [SerializeField] private Image equipEffect;
    [SerializeField] private Image gradeEffect;

    public Color normalColor = new Color(1f, 1f, 1f);
    public Color rareColor = new Color(0.2f, 0.6f, 1f);
    public Color uniqueColor = new Color(0.7f, 0.3f, 1f);
    public Color heroColor = new Color(1f, 0.5f, 0.1f);
    public Color legendColor = new Color(1f, 0.84f, 0f);

    private ItemInstance data;

    public void SetData(ItemInstance newData)
    {
        data = newData;
        Refresh();
    }

    public void Clear()
    {
        data = null;
        SetIcon(false);
        SetSelected(false);
        SetEquipped(false);
        SetGradeEffect();
    }

    public void Refresh()
    {
        if (data == null || data.Data == null)
        {
            Clear();
            return;
        }
        icon.sprite = data.Data.Icon;
        SetIcon(true);
        SetGradeEffect();
    }

    public void SetIcon(bool active) => icon?.gameObject?.SetActive(active);
    public void SetEquipped(bool equipped) => equipEffect?.gameObject?.SetActive(equipped);
    public void SetSelected(bool selected) => selectedEffect?.gameObject?.SetActive(selected);

    public void SetGradeEffect()
    {
        if (data?.Data == null)
        {
            gradeEffect?.gameObject?.SetActive(false);
            return;
        }

        gradeEffect.gameObject.SetActive(true);
        switch (data.Data.ItemGrade)
        {
            case ItemGrade.Normal:
                gradeEffect.color = normalColor;
                break;
            case ItemGrade.Rare:
                gradeEffect.color = rareColor;
                break;
            case ItemGrade.Unique:
                gradeEffect.color = uniqueColor;
                break;
            case ItemGrade.Hero:
                gradeEffect.color = heroColor;
                break;
            case ItemGrade.Legend:
                gradeEffect.color = legendColor;
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager inventoryManager = MainSceneManager.Instance.inventoryManager;
        if (data == null || inventoryManager == null) return;

        if (inventoryManager.inventorySelectionController.selectionMode == SelectionMode.Single)
        {
            inventoryManager.inventorySelectionController.SetSelected(this);
            //if (inventoryManager.inventoryUIManager.upgradePopupController.IsOpen)
            //{
            //    inventoryManager.inventoryUIManager.OpenPopup(PopupType.Upgrade);
            //    return;
            //}
            inventoryManager.inventoryUIManager.OpenPopup(PopupType.Item);
        }
        else
        {
            inventoryManager.inventorySelectionController.SelectSlotList(this);
        }
    }

    public ItemInstance GetData() => data;
}
