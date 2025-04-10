using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryUIButton : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [field: SerializeField] public Button allBtn { get; private set; }
    [field: SerializeField] public Button weaponBtn { get; private set; }
    [field: SerializeField] public Button helmetBtn { get; private set; }
    [field: SerializeField] public Button armorBtn { get; private set; }
    [field: SerializeField] public Button shoesBtn { get; private set; }
    [field: SerializeField] public Button ringBtn { get; private set; }
    [field: SerializeField] public Button necklaceBtn { get; private set; }

    public UnityEvent<EquipType> bindInventoryBtn;

    private void Awake()
    {
        allBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(EquipType.Count));
        weaponBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(EquipType.Weapon));
        helmetBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(EquipType.Helmet));
        armorBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(EquipType.Armor));
        shoesBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(EquipType.Shoes));
        ringBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(EquipType.Ring));
        necklaceBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(EquipType.Necklace));
    }

}
