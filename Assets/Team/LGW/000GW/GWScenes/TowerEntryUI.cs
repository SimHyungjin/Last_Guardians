using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerEntryUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI towerNameText;
    public TextMeshProUGUI descriptionText; // �� ���� �ϳ��� ����
    public Button entryButton;

    private TowerData myData;

    public void SetData(TowerData data)
    {
        myData = data;

        towerNameText.text = data.TowerName;
        descriptionText.text = data.TowerDescription; // �� ����� ��ü

        int spriteIndex = GetSpriteIndex(data.TowerIndex);
        icon.sprite = data.atlas?.GetSprite($"Tower_{spriteIndex}");

        entryButton.onClick.RemoveAllListeners();
        entryButton.onClick.AddListener(() =>
        {
            if (TowerCombinationUI.Instance.HasCombinationFor(data))
            {
                TowerCombinationUI.Instance.ShowCombinationFor(data);
                
            }
        });
    }

    private int GetSpriteIndex(int index)
    {
        //return Utils.GetSpriteIndex(index); // �� �� �κ��� Utils Ŭ�������� ó��
        if (index > 49 && index < 99) return index - 49;
        if (index > 98 && index < 109) return index - 98;
        if (index > 108) return index - 59;
        return index;
    }
}
