using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Season
{
    spring,
    summer,
    autumn,
    winter,
    All
}

public class EnviromentManager : Singleton<EnviromentManager>
{
    //계절 판별
    //날씨 판별
    //플랫폼 배치

    private Platform platform;
    public List<Platform> Platforms { get; private set; } = new List<Platform>();
    public WeatherState WeatherState { get; private set; }
    public Season Season { get; private set; }

    private Coroutine stateCorutine;
    private WaitForSeconds seconds = new WaitForSeconds(0.2f);

    private void Start()
    {
        WeatherState = new WeatherState();
        stateCorutine = StartCoroutine(StateUpdate());
        platform = Resources.Load<Platform>("Enviroment/Platform");
        //SetSeason(GameManager.Instance.NowTime);
        WeatherState.WeatherListInit(EnviromentManager.Instance.Season);

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
        else
        {
            Season = Season.winter;
        }

        Debug.Log($"현재계절 : {Season}");
    }

   
    IEnumerator StateUpdate()
    {
        WeatherState.Update();

        yield return seconds;
    }

    private void OnDisable()
    {
        StopCoroutine(stateCorutine);
        stateCorutine = null;
    }
}
