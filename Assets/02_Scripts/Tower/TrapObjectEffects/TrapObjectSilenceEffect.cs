using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapObjectSilenceEffect : ITrapEffect
{
    ///////////==========================ħ�� ����Ʈ================================/////////////////////

    public void Apply(BaseMonster target, TowerData towerData, bool bossImmunebuff,EnvironmentEffect environmentEffect)
    {
        if(target.MonsterData.MonsterType==MonType.Boss) target= target.GetComponent<BossMonster>();
    }
}
