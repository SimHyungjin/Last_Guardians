using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FogWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log("날씨상태 : 안개");
    }

    public void Exit()
    {
        
    }
    public void Update()
    {
        
    }

}

public class StrongWindWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log("날씨상태 : 강풍");
    }

    public void Exit()
    {
       
    }

    public void Update()
    {
        
    }
}

public class RainWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log("날씨상태 : 비");
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}

public class DroughtWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log("날씨상태 : 가뭄");
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
    public void Enter()
    {
        Debug.Log("날씨상태 : 눈");
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}

public class SunnyWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log("날씨상태 : 맑음(none)");
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}
