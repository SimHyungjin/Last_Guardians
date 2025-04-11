using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject selectedEffect;
    [SerializeField] private GameObject equipEffect;
    [SerializeField] private GameObject gradeEffect;

    private Image gradeEffectImage;
    private ItemData data;

    public bool isSelected { get; private set; } = false;

    private void Awake()
    {
        icon = GetComponent<Image>();
        gradeEffectImage = gradeEffect.GetComponent<Image>();
    }

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
        SetGradeEffect();
    }

    public void Refresh()
    {
        if (data == null)
        {
            Clear();
            return;
        }
        SetGradeEffect();
    }

    public void SetEquipped(bool equipped) => equipEffect?.SetActive(equipped);
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        if (selectedEffect != null) selectedEffect.SetActive(selected);
    }
    public void SetGradeEffect()
    {
        if (gradeEffect != null) gradeEffect.SetActive(false);
        if (data == null || gradeEffect == null) return;
        if ((int)data.itemGrade == 0) return;

        gradeEffect.SetActive(true);
        float color = 0.2f * (int)data.itemGrade;
        gradeEffectImage.color = new Color(color, 0, 0, 0.5f);
    }

    

    public void OnPointerClick(PointerEventData eventData)
    {
        if (data == null) return;
        HomeManager.Instance.selectionController.SelectSlot(this);
    }

    public ItemData GetData() => data;
}
