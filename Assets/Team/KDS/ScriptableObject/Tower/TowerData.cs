using UnityEngine;
public enum TowerType
{
    Elemental,
    Standard,
    Fusion,
    Ultimate
}


[CreateAssetMenu(fileName = "NewTower", menuName = "Data/Tower Data", order = 1)]
public class TowerData: ScriptableObject
{
    [Header("Å¸¿ö ÇÁ¸®ÆÕ")]
    [SerializeField] public GameObject towerGhostPrefab;

    [Header("Å¸¿ö ½ºÅÝ")]
    [SerializeField] private int towerIndex;
    [SerializeField] private string towerName;
    [SerializeField] private float attackPower;           
    [SerializeField] private float attackSpeed;          
    [SerializeField] private float attackRange;
    [SerializeField] private TowerType towerType; 
    [SerializeField] private string specialEffect;        
    [SerializeField] private float effectPower;           
    [SerializeField] private string synergy;              
    [SerializeField] private int projectileType;

    public int TowerIndex => towerIndex;
    public string TowerName => towerName;
    public float AttackPower => attackPower;
    public float AttackSpeed => attackSpeed;
    public float AttackRange => attackRange;
    public TowerType TowerType => towerType;
    public string SpecialEffect => specialEffect;
    public float EffectPower => effectPower;
    public string Synergy => synergy;
    public int ProjectileType => projectileType;
    public int upgradeLevel { get; private set; }          

    public void SetData(int towerIndex,string towerName, float attackPower, float attackSpeed, float attackRange, TowerType towerType,
                        string specialEffect, float effectPower, int upgradeLevel, string synergy, int projectileType)
    {
        this.towerIndex = towerIndex;
        this.towerName = towerName;
        this.attackPower = attackPower;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.towerType = towerType;
        this.specialEffect = specialEffect;
        this.effectPower = effectPower;
        this.synergy = synergy;
        this.projectileType = projectileType;
        this.upgradeLevel = upgradeLevel;
    }
}

