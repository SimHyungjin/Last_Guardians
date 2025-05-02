using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FogWeather : IWeatherState
{
    PlayerBuffAttackRange playerAttackRange = new PlayerBuffAttackRange(-0.25f, 0.2f);
    
    public void Enter()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name}");
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Fog"));
        particle.gameObject.SetActive(true);
        InGameManager.Instance.playerManager.playerHandler.playerBuffHandler.ApplyBuff(playerAttackRange); //플레이어
    }

    public void Exit()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name} 종료");
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Fog"));
        particle.gameObject.SetActive(false);
        InGameManager.Instance.playerManager.playerHandler.playerBuffHandler.RemoveBuff(playerAttackRange);
    }
    public void Update()
    {
        InGameManager.Instance.playerManager.playerHandler.playerBuffHandler.ApplyBuff(playerAttackRange);

    }
}

public class StrongWindWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name}");
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Wind"));
        particle.gameObject.SetActive(true);

        foreach (var obs in EnviromentManager.Instance.Obstacles)// 장애물
        {
            obs.Init(Weather.Default);
        }

        foreach (BaseTower tower in TowerManager.Instance.Towers)
        {
            AttackTower attackTower = tower as AttackTower;
            if (attackTower == null) continue;
            if (attackTower.towerData.ElementType == ElementType.Wind || attackTower.isSpeedBuffed)
                attackTower.OnWindSpeedBuff();

        }
    }

    public void Exit()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name} 종료");
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Wind"));
        particle.gameObject.SetActive (false);
        foreach (BaseTower tower in TowerManager.Instance.Towers)
        {
            AttackTower attackTower = tower as AttackTower;
            if (attackTower == null) continue;
            if (attackTower.isWindBuffed)
                attackTower.OffWindSpeedBuff();
        }
    }

    public void Update()
    {

    }
}

public class RainWeather : IWeatherState
{
    
    public void Enter()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name}");
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Rain"));
        particle.gameObject.SetActive(true);

        foreach (var monsters in MonsterManager.Instance.AlliveMonsters) // 몬스터
        {
            monsters.ApplySlowdown(0.9f, 1f);
        }



        foreach (var obs in EnviromentManager.Instance.Obstacles) // 장애물
        {
            obs.Init(Weather.Default);
        }
    }

    public void Exit()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name} 종료");
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Rain"));
        particle.gameObject.SetActive(false);
        foreach (var monsters in MonsterManager.Instance.AlliveMonsters)
        {
            monsters.CancelSlowdown();
        }
    }

    public void Update()
    {
        foreach (var monsters in MonsterManager.Instance.AlliveMonsters)
        {
            monsters.ApplySlowdown(0.9f, 0.2f);
        }
    }
}

public class DroughtWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name}");
        foreach (var obs in EnviromentManager.Instance.Obstacles)
        {
            obs.Init(Weather.Drought);
        }
    }

    public void Exit()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name} 종료");
    }

    public void Update()
    {

    }
}

public class SnowWeather : IWeatherState
{
    PlayerBuffMoveSpeed playerBuffMoveSpeed = new PlayerBuffMoveSpeed(0.3f, 0.2f, true);
    
    public void Enter()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name}");
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Snow"));
        particle.gameObject.SetActive(true);
        InGameManager.Instance.playerManager.playerHandler.playerBuffHandler.ApplyBuff(playerBuffMoveSpeed);
        foreach (var monsters in MonsterManager.Instance.AlliveMonsters)
        {
            monsters.ApplySlowdown(0.85f, 1f);
        }
        foreach (var obs in EnviromentManager.Instance.Obstacles)
        {
            obs.Init(Weather.Snow);
        }
    }

    public void Exit()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name} 종료");
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Snow"));
        particle.gameObject.SetActive(false);
        InGameManager.Instance.playerManager.playerHandler.playerBuffHandler.RemoveBuff(playerBuffMoveSpeed);
        foreach (var monsters in MonsterManager.Instance.AlliveMonsters)
        {
            monsters.CancelSlowdown();
        }
    }

    public void Update()
    {
        InGameManager.Instance.playerManager.playerHandler.playerBuffHandler.ApplyBuff(playerBuffMoveSpeed);
        foreach (var monsters in MonsterManager.Instance.AlliveMonsters)
        {
            monsters.ApplySlowdown(0.85f, 0.2f);
        }
    }
}

public class SunnyWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name}");
        foreach (var obs in EnviromentManager.Instance.Obstacles)
        {
            obs.Init(Weather.Default);
        }
    }

    public void Exit()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name} 종료");
    }

    public void Update()
    {

    }
}
