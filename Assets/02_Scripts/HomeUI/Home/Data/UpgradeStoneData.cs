using UnityEngine;

public enum UpgradeStoneEffect
{
    None,
    Downgrade,
}
[CreateAssetMenu(menuName = "Data/UpgradeStoneData", fileName = "UpgradeStoneData")]
public class UpgradeStoneData : ItemData
{
    public int upgradeIndex;
    public ItemGrade sourceGrade;
    public ItemGrade targetGrade;
    public int requiredUpgradeStones;
    public int requiredGold;
    public float successRate;
    public UpgradeStoneEffect failureEffect;
}