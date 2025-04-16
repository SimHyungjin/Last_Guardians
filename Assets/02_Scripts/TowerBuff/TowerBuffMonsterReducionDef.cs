public class TowerBuffMonsterReducionDef : ITowerBuff
{
    public void ApplyBuff(BaseTower tower, TowerData data)
    {

    }
    public void ApplyDebuff(BaseMonster monster, TowerData data)
    {
        monster.ApplyReducionDef(data.EffectValue, 0.1f);
    }
}
