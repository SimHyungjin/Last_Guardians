using UnityEngine;

public enum UpgradeFailureEffect
{
    None,
    Downgrade
}

[CreateAssetMenu(menuName = "Data/UpgradeRuleData", fileName = "NewUpgradeRuleData")]
public class UpgradeRuleData : ScriptableObject
{
    public int upgradeIndex;
    public ItemGrade sourceGrade;
    public ItemGrade targetGrade;
    public int requiredUpgradeStones;
    public int requiredGold;
    public float successRate;
    public UpgradeFailureEffect failureEffect;
}
