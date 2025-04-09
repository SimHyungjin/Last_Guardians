using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private ItemData data;

    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    public void SetData(ItemData newData)
    {
        data = newData;
        UpdateSlotUI();
    }

    public void ClearData()
    {
        data = null; 
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        if (data == null)
        {
            icon.sprite = null;
        }
        else
        {
            icon.sprite = data.icon;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (data == null) return;
        HomeManager.Instance.SetSelectedItem(this);
    }

    public void EffectEnable()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void EffectDisable()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public ItemData Getdata() => data;
}
