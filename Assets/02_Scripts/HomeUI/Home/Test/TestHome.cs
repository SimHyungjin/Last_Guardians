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
        var inventory = HomeManager.Instance.inventory;
        var container = HomeManager.Instance.inventorySlotContainer;

        // 인벤토리 초기화 (선택사항: 매번 새로 채우고 싶다면)
        inventory.ClearAll(); // 이 메서드가 없다면 따로 만들어야 함

        // 랜덤으로 5개 고르기
        List<ItemData> chosen = GetRandomItems(itemData, 5);

        foreach (var item in chosen)
            inventory.AddItem(item);

        container.Display(inventory.GetFilteredView());
    }

    private List<ItemData> GetRandomItems(ItemData[] source, int count)
    {
        List<ItemData> shuffled = new List<ItemData>(source);
        int n = shuffled.Count;

        // Fisher–Yates 셔플
        for (int i = 0; i < n - 1; i++)
        {
            int j = Random.Range(i, n);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        return shuffled.GetRange(0, Mathf.Min(count, shuffled.Count));
    }
}
