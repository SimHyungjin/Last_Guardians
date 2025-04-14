using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Equipment,
    UpgradeStone,
}

public enum ItemGrade
{
    Normal,
    Rare,
    Unique,
    Hero,
    Legend
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Data/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    [SerializeField] private int itemIndex;
    [SerializeField] public string itemName;
    [SerializeField] public string itemDescript;
    [SerializeField] private ItemType itemType;
    [SerializeField] public ItemGrade itemGrade;
    [SerializeField] private int itemStackLimit;
    [SerializeField] private float itemDropRate;
    [SerializeField] private int itemSellPrice;
    [SerializeField] public Sprite icon;

    public int ItemIndex => itemIndex;
    public string ItemName => itemName;
    public string ItemDescript => itemDescript;
    public ItemType ItemType => itemType;
    public ItemGrade ItemGrade => itemGrade;
    public int ItemStackLimit => itemStackLimit;
    public float ItemDropRate => itemDropRate;
    public int ItemSellPrice => itemSellPrice;
    public Sprite Icon => icon;

    public void SetData(int index, string name, string descript, ItemType type, ItemGrade grade, int stackLimit, float dropRate, int sellPrice)
    {
        itemIndex = index;
        itemName = name;
        itemDescript = descript;
        itemType = type;
        itemGrade = grade;
        itemStackLimit = stackLimit;
        itemDropRate = dropRate;
        itemSellPrice = sellPrice;
    }
    [CustomEditor(typeof(EquipData))]
    public class EquipDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EquipData equipData = (EquipData)target;

            
            EditorGUILayout.LabelField("Item Name", equipData.ItemName);

           
            DrawDefaultInspector();
        }
    }
}