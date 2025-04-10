using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject selectedEffect;
    [SerializeField] private GameObject equipEffect;
    [SerializeField] private GameObject gradeEffect;

    private ItemData data;

    public void SetData(ItemData newData)
    {
        data = newData;
        icon.sprite = data ? data.icon : null;
    }

    public void Clear()
    {
        SetData(null);
        SetSelected(false);
        SetEquipped(false);
        SetGradeEffect(false);
    }

    public void SetSelected(bool selected) => selectedEffect?.SetActive(selected);
    public void SetEquipped(bool equipped) => equipEffect?.SetActive(equipped);

    public void SetGradeEffect(bool isActive)
    {
        if (gradeEffect == null || data == null) return;
        if ((int)data.itemGrade == 0) return;
        gradeEffect.SetActive(isActive);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (data == null) return;
        HomeManager.Instance.selectionController.SelectSlot(this);
    }

    public ItemData GetData() => data;
}
