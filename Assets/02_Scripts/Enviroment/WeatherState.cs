using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeatherState
{
    void Enter(); //진입 시
    void Exit(); //탈출 시
    void Update();//업데이트에 들어갈꺼
}

public class WeatherState
{
    private IWeatherState currentState;

    private FogWeather fogWeather = new FogWeather();
    private StrongWindWeather strongWindWeather = new StrongWindWeather();
    private RainWeather rainWeather = new RainWeather();
    private DroughtWeather droughtWeather = new DroughtWeather();
    private SnowWeather snowWeather = new SnowWeather();
    private SunnyWeather sunnyWeather = new SunnyWeather();

    private List<(IWeatherState, float weight)> weatherList = new List<(IWeatherState, float wegiht)>();


    public void SetWeather()
    {
        ChangeState(GetRandomWeather(weatherList));
    }

    public void WeatherListInit(Season season)
    {
        switch (season)
        {
            case Season.spring:
                weatherList.Add((fogWeather, 15f));
                weatherList.Add((strongWindWeather, 25f));
                weatherList.Add((sunnyWeather, 60f));
                break;

            case Season.summer:
                weatherList.Add((fogWeather, 10f));
                weatherList.Add((rainWeather, 25f));
                weatherList.Add((droughtWeather, 15f));
                weatherList.Add((sunnyWeather, 50f));
                break;

            case Season.autumn:
                weatherList.Add((fogWeather, 15f));
                weatherList.Add((strongWindWeather, 25f));
                weatherList.Add((sunnyWeather, 60f));
                break;

            case Season.winter:
                weatherList.Add((fogWeather, 15f));
                weatherList.Add((snowWeather, 30f));
                weatherList.Add((sunnyWeather, 55f));
                break;
        }
    }
    private void ChangeState(IWeatherState newState)
    {
        if (currentState == newState)
        {
            Debug.Log("날씨 변화 없음");
            return;
        }
            
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    //가중치에 따라서 날씨 상태 뽑아내기
    private IWeatherState GetRandomWeather(List<(IWeatherState weather, float weight)> weathers)
    {
        if (weathers == null || weathers.Count == 0)
        {
            Debug.LogError("날씨 리스트가 비어있음");
            return null;
        }

        float totalWeight = 0f;

        // 전체 가중치 합산
        foreach (var item in weathers)
        {
            totalWeight += item.weight;
        }

        // 0 ~ totalWeight 사이의 랜덤값
        float randomValue = Random.Range(0f, totalWeight);

        float cumulativeWeight = 0f;

        foreach (var item in weathers)
        {
            cumulativeWeight += item.weight;
            if (randomValue <= cumulativeWeight)
            {
                return item.weather;
            }
        }

        //예외처리
        return weathers[0].weather;
    }

    public void Update()
    {
        currentState?.Update();
    }

    public IWeatherState GetCurrentState()
    {
        return currentState;
    }

    public string GetSeasonText(string str)
    {
        switch (EnviromentManager.Instance.Season)
        {
            case Season.spring:
                str = "봄";
                break;
            case Season.summer:
                str = "여름";
                break;
            case Season.autumn:
                str = "가을";
                break;
            case Season.winter:
                str = "겨울";
                break;
        }
        return str;
    }

    public string GetWeatherName()
    {
        string str = GetCurrentState().ToString();
        switch (str)
        {
            case "FogWeather":
                str = "안개";
                break;
            case "StrongWindWeather":
                str = "강풍";
                break;
            case "RainWeather":
                str = "비";
                break;
            case "DroughtWeather":
                str = "가뭄";
                break;
            case "SnowWeather":
                str = "눈";
                break;
            case "SunnyWeather":
                str = "맑음";
                break;
        }
        return str;
    }
}
