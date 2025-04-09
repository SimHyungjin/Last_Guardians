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

    public UnityEvent<ItemType> bindInventoryBtn;

    private void Awake()
    {
        allBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(ItemType.Count));
        weaponBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(ItemType.Weapon));
        helmetBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(ItemType.Helmet));
        armorBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(ItemType.Armor));
        shoesBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(ItemType.Shoes));
        ringBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(ItemType.Ring));
        necklaceBtn.onClick.AddListener(() => bindInventoryBtn?.Invoke(ItemType.Necklace));
    }

}
