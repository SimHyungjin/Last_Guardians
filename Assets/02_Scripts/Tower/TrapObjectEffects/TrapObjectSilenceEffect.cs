public class TrapObjectSilenceEffect : ITrapEffect
{
    ///////////==========================침묵 이펙트================================/////////////////////

    public void Apply(BaseMonster target, TowerData towerData, AdaptedTrapObjectData adaptedTrapObjectData, bool bossImmunebuff, EnvironmentEffect environmentEffect)
    {
        if (target.MonsterData.MonsterType == MonType.Boss) target = target.GetComponent<BossMonster>();
    }
}
