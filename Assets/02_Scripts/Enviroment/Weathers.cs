using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log("날씨상태 : 안개");
    }
}

public class StrongWindWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log("날씨상태 : 강풍");
    }
}

public class RainWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log("날씨상태 : 비");
    }
}

public class DroughtWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log("날씨상태 : 가뭄");
    }
}

public class SnowWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log("날씨상태 : 눈");
    }
}

public class SunnyWeather : IWeatherState
{
    public void Enter()
    {
        Debug.Log("날씨상태 : 맑음(none)");
    }
}
