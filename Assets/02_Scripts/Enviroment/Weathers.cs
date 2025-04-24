using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FogWeather : IWeatherState
{
    PlayerBuffAttackRange playerAttackRange = new PlayerBuffAttackRange(-0.25f, 0.2f);
    public void Enter()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name}");

        InGameManager.Instance.playerManager.playerController.playerBuffHandler.ApplyBuff(playerAttackRange); //플레이어
    }

    public void Exit()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name} 종료");
        InGameManager.Instance.playerManager.playerController.playerBuffHandler.RemoveBuff(playerAttackRange);
    }
    public void Update()
    {
        InGameManager.Instance.playerManager.playerController.playerBuffHandler.ApplyBuff(playerAttackRange);

    }
}

public class StrongWindWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name}");

        foreach (var obs in EnviromentManager.Instance.Obstacles)// 장애물
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

public class RainWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name}");

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
    PlayerBuffMoveSpeed playerBuffMoveSpeed = new PlayerBuffMoveSpeed(-15f, 0.2f, true);
    public void Enter()
    {
        Debug.Log($"날씨상태 : {this.GetType().Name}");
        InGameManager.Instance.playerManager.playerController.playerBuffHandler.ApplyBuff(playerBuffMoveSpeed);
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
        InGameManager.Instance.playerManager.playerController.playerBuffHandler.RemoveBuff(playerBuffMoveSpeed);
        foreach (var monsters in MonsterManager.Instance.AlliveMonsters)
        {
            monsters.CancelSlowdown();
        }
    }

    public void Update()
    {
        InGameManager.Instance.playerManager.playerController.playerBuffHandler.ApplyBuff(playerBuffMoveSpeed);
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
