using UnityEngine;
using UnityEngine.U2D;
public enum TowerType
{
    Elemental,
    Standard,
    Fusion,
    Ultimate
}
public enum ProjectileType
{
    Magic,
    Arrow,
    AreaAttack,
    AreaBuff,
    CreateObgect
}
public enum SpecialEffect
{
    DotDamage, 
    Slow, 
    MultyTarget, 
    ChainAttack, 
    Stun, 
    BossDamage, 
    BossDebuff, 
    DefReduc, 
    Knockback, 
    Buff, 
    AttackPower, 
    AttackSpeed, 
    Summon,
    None
}
public enum EffectTarget
{
    Single,     
    All,       
    Multiple,  
    BossOnly,   
    Towers  
}

[CreateAssetMenu(fileName = "NewTower", menuName = "Data/Tower Data", order = 1)]
public class TowerData: ScriptableObject
{
    [Header("타워 외관")]
    [SerializeField] public GameObject towerGhostPrefab;
    [SerializeField] public SpriteAtlas atlas;

    [Header("타워 스텟")]
    [SerializeField] private int towerIndex;
    [SerializeField] private string towerName;
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private TowerType towerType;
    [SerializeField] private ProjectileType projectileType;
    [SerializeField] private SpecialEffect specialEffect;
    [SerializeField] private float effectChance;
    [SerializeField] private float effectDuration;
    [SerializeField] private float effectValue;
    [SerializeField] private EffectTarget effectTarget;
    [SerializeField] private int effectTargetCount;
    [SerializeField] private bool bossImmune;
    [SerializeField] private int upgradeLevel;
    [SerializeField] private string towerDescription;

    public int TowerIndex => towerIndex;
    public string TowerName => towerName;
    public float AttackPower => attackPower;
    public float AttackSpeed => attackSpeed;
    public float AttackRange => attackRange;
    public TowerType TowerType => towerType;
    public ProjectileType ProjectileType => projectileType;
    public SpecialEffect SpecialEffect => specialEffect;
    public float EffectChance => effectChance;
    public float EffectDuration => effectDuration;
    public float EffectValue => effectValue;
    public EffectTarget EffectTarget => effectTarget;
    public int EffectTargetCount => effectTargetCount;
    public bool BossImmune => bossImmune;
    public int UpgradeLevel => upgradeLevel;
    public string TowerDescription => towerDescription;

    public void SetData(int towerIndex, string towerName, float attackPower, float attackSpeed,
                        float attackRange, TowerType towerType, ProjectileType projectileType,
                        SpecialEffect specialEffect, float effectChance, float effectDuration,
                        float effectValue, EffectTarget effectTarget, int effectTargetCount,
                        bool bossImmune, int upgradeLevel, string towerDescription)
    {
        Debug.Log("SetData called with towerIndex: " + towerIndex);
        this.towerIndex = towerIndex;
        this.towerName = towerName;
        this.attackPower = attackPower;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.towerType = towerType;
        this.projectileType = projectileType;
        this.specialEffect = specialEffect;
        this.effectChance = effectChance;
        this.effectDuration = effectDuration;
        this.effectValue = effectValue;
        this.effectTarget = effectTarget;
        this.effectTargetCount = effectTargetCount;
        this.bossImmune = bossImmune;
        this.upgradeLevel = upgradeLevel;
        this.towerDescription = towerDescription;
    }
}

