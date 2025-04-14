using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Image selectedEffect;
    [SerializeField] private Image equipEffect;
    [SerializeField] private Image gradeEffect;

    [SerializeField] private ItemData data;

    public void SetData(ItemData newData)
    {
        data = newData;
        icon.sprite = data ? data.icon : null;
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

    public void SetIcon(bool active) => icon.gameObject?.SetActive(active);
    public void SetEquipped(bool equipped) => equipEffect.gameObject?.SetActive(equipped);
    public void SetSelected(bool selected) => selectedEffect.gameObject?.SetActive(selected);
    public void SetGradeEffect()
    {
        if (data == null)
        {
            if (gradeEffect != null) gradeEffect.gameObject.SetActive(false);
            return;
        }
        gradeEffect.gameObject.SetActive(true);
        switch (data.itemGrade)
        {
            case ItemGrade.Normal:
                gradeEffect.gameObject.SetActive(false);
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
        HomeManager.Instance.selectionController.SelectSlot(this);
    }

    public ItemData GetData() => data;
}
