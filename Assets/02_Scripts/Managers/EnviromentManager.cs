using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Season
{
    spring,
    summer,
    autumn,
    winter
}

public class EnviromentManager : Singleton<EnviromentManager>
{
    //계절 판별
    //날씨 판별
    //플랫폼 배치

    public WeatherState WeatherState { get; private set; }
    public Season Season { get; private set; }

    private void Start()
    {
        WeatherState = new WeatherState();
    }

    public void SetSeason(int min)
    {
        if (min >= 0 && min < 15)
        {
            Season = Season.spring;
        }
        else if (min >= 15 && min < 30)
        {
            Season = Season.summer;
        }
        else if (min >= 30 && min < 45)
        {
            Season = Season.autumn;
        }
        else if (min >= 45 && min < 60)
        {
            Season = Season.winter;
        }

        Debug.Log($"현재계절 : {Season}");
    }
}
