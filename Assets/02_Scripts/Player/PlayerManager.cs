public class PlayerManager
{
    public PlayerStatus playerStatus { get; private set; }
    public EquipmentStat equipmentStat { get; private set; }
    public PlayerHandler playerHandler { get; private set; }

    /// <summary>
    /// 플레이어 스탯 객체를 초기화합니다.
    /// </summary>
    public void InitBaseStat()
    {
        playerStatus = new PlayerStatus();
        equipmentStat = new EquipmentStat();
        playerStatus.Init();
    }

    public void SaveEquipmentStat(EquipmentStat equipment)
    {
        equipmentStat = equipment;
    }

    /// <summary>
    /// 플레이어 프리팹을 생성하고 핸들러를 초기화합니다.
    /// </summary>
    public void SpawnPlayerModel()
    {
        playerHandler = Utils.InstantiatePrefabFromResource("Player/Player").GetComponent<PlayerHandler>();
        playerStatus.SetStatus();
        playerHandler.weaponHandler.SetWeapon(playerStatus.attackType, equipmentStat.icon);
        playerHandler.Init(playerStatus);
    }
}
