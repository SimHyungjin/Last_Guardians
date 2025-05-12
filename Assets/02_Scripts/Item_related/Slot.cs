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

    private ItemInstance data;

    public void SetData(ItemInstance newData)
    {
        data = newData;
        icon.sprite = data?.Data?.icon;
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
        if (data == null)
        {
            Clear();
            return;
        }

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
        switch (data.Data.itemGrade)
        {
            case ItemGrade.Normal:
                gradeEffect.color = new Color(1f, 1f, 1f);
                break;
            case ItemGrade.Rare:
                gradeEffect.color = new Color(0.2f, 0.6f, 1f);
                break;
            case ItemGrade.Unique:
                gradeEffect.color = new Color(0.7f, 0.3f, 1f);
                break;
            case ItemGrade.Hero:
                gradeEffect.color = new Color(1f, 0.5f, 0.1f);
                break;
            case ItemGrade.Legend:
                gradeEffect.color = new Color(1f, 0.84f, 0f);
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (data == null) return;
        SelectionController selectionController = MainSceneManager.Instance.inventoryGroup.itemConnecter.selectionController;
        if(selectionController.selectionMode == SelectionMode.Single)selectionController.SelectSlot(this);
        else selectionController.SelectSlotList(this);
    }

    public ItemInstance GetData() => data;
}
