using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject selectedEffect;
    [SerializeField] private GameObject equipEffect;
    [SerializeField] private GameObject gradeEffect;

    [SerializeField] private ItemData data;

    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    public void SetData(ItemData newData)
    {
        data = newData;
        icon.sprite = data ? data.icon : null;
    }

    public void SetSelected(bool selected)
    {
        if (selectedEffect == null) return;
        selectedEffect.SetActive(selected);
    }

    public void SetEquipped(bool equipped)
    {
        if (equipEffect == null) return;
        equipEffect.SetActive(equipped);
    }

    public void SetGradeEffect(bool isActive)
    {
        if (gradeEffect == null || data == null) return;
        if ((int)data.itemGrade == 0) return;
        gradeEffect.SetActive(isActive);
    }

    public void Clear()
    {
        SetData(null);
        SetSelected(false);
        SetEquipped(false);
        SetGradeEffect(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (data == null) return;
        HomeManager.Instance.selectionController.SelectSlot(this);
    }

    public ItemData GetData() => data;
}
