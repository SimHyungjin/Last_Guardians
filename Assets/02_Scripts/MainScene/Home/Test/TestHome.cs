using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestHome : MonoBehaviour
{
    public Button btn;
    public ItemData[] itemData;

    private void Start()
    {
        btn.onClick.AddListener(Test);
    }

    public void Test()
    {
        var inventory = MainSceneManager.Instance.inventory;
        var container = MainSceneManager.Instance.inventoryGroup.inventorySlotContainer;

        inventory.ClearAll();

        // 랜덤으로 5개 고르고 인스턴스로 변환해서 추가
        List<ItemData> chosen = GetRandomItems(itemData, 5);

        foreach (var item in chosen)
        {
            var instance = new ItemInstance(item);
            inventory.AddItem(instance);
        }

        container.Display(inventory.GetFilteredView());
    }

    private List<ItemData> GetRandomItems(ItemData[] source, int count)
    {
        List<ItemData> shuffled = new List<ItemData>(source);
        int n = shuffled.Count;

        for (int i = 0; i < n - 1; i++)
        {
            int j = Random.Range(i, n);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        return shuffled.GetRange(0, Mathf.Min(count, shuffled.Count));
    }
}
