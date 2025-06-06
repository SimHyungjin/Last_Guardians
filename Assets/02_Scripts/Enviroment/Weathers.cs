using UnityEngine;

public class FogWeather : IWeatherState
{
    PlayerBuffAttackRange playerAttackRange = new PlayerBuffAttackRange(-0.25f, 0.2f);

    public void Enter()
    {
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Fog"));
        particle.gameObject.SetActive(true);
        GameManager.Instance.PlayerManager.playerHandler.playerBuffHandler.ApplyBuff(playerAttackRange); //플레이어
    }

    public void Exit()
    {
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Fog"));
        particle.gameObject.SetActive(false);
        GameManager.Instance.PlayerManager.playerHandler.playerBuffHandler.RemoveBuff(playerAttackRange);
    }
    public void Update()
    {
        GameManager.Instance.PlayerManager.playerHandler.playerBuffHandler.ApplyBuff(playerAttackRange);

    }
}

public class StrongWindWeather : IWeatherState
{
    public void Enter()
    {
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
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Wind"));
        particle.gameObject.SetActive(false);
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
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Rain"));
        particle.gameObject.SetActive(true);

        foreach (var monsters in MonsterManager.Instance.AlliveMonsters) // 몬스터
        {
            monsters.ApplySlowdown(0.1f, 1f);
        }



        foreach (var obs in EnviromentManager.Instance.Obstacles) // 장애물
        {
            obs.Init(Weather.Default);
        }
    }

    public void Exit()
    {
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
            monsters.ApplySlowdown(0.1f, 0.2f);
        }
    }
}

public class DroughtWeather : IWeatherState
{
    public void Enter()
    {
        foreach (var obs in EnviromentManager.Instance.Obstacles)
        {
            obs.Init(Weather.Drought);
        }
    }

    public void Exit()
    {

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
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Snow"));
        particle.gameObject.SetActive(true);
        GameManager.Instance.PlayerManager.playerHandler.playerBuffHandler.ApplyBuff(playerBuffMoveSpeed);
        foreach (var monsters in MonsterManager.Instance.AlliveMonsters)
        {
            monsters.ApplySlowdown(0.15f, 1f);
        }
        foreach (var obs in EnviromentManager.Instance.Obstacles)
        {
            obs.Init(Weather.Snow);
        }
    }

    public void Exit()
    {
        ParticleSystem particle = EnviromentManager.Instance.Particles.Find(a => a.gameObject.name.Contains("FX_Snow"));
        particle.gameObject.SetActive(false);
        GameManager.Instance.PlayerManager.playerHandler.playerBuffHandler.RemoveBuff(playerBuffMoveSpeed);
        foreach (var monsters in MonsterManager.Instance.AlliveMonsters)
        {
            monsters.CancelSlowdown();
        }
    }

    public void Update()
    {
        GameManager.Instance.PlayerManager.playerHandler.playerBuffHandler.ApplyBuff(playerBuffMoveSpeed);
        foreach (var monsters in MonsterManager.Instance.AlliveMonsters)
        {
            monsters.ApplySlowdown(0.15f, 0.2f);
        }
    }
}

public class SunnyWeather : IWeatherState
{
    public void Enter()
    {
        foreach (var obs in EnviromentManager.Instance.Obstacles)
        {
            obs.Init(Weather.Default);
        }
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }
}
