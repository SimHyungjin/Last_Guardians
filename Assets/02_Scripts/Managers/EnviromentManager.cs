using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public enum Season
{
    spring,
    summer,
    autumn,
    winter,
    Default,
    All
}

public class EnviromentManager : Singleton<EnviromentManager>
{
    public WeatherState WeatherState { get; private set; }
    public Season Season { get; private set; }

    public List<ParticleSystem> Particles { get; private set; } = new();

    private List<GameObject> firstObjectTemplates = new();
    private List<GameObject> secondObjectTemplates = new();
    private List<GameObject> thirdObjectTemplates = new();
    private List<GameObject> fourthObjectTemplates = new();
    
    public List<GameObject> MapPrefabs { get; private set; } = new();

    public List<BaseObstacle> Obstacles { get; private set; } = new();

    public int WeatherCycle { get; private set; } = 5;

    private Coroutine stateCorutine;
    private static readonly WaitForSeconds waitForSeconds = new(0.2f);

    private void Start()
    {
        WeatherState = new WeatherState();
        stateCorutine = StartCoroutine(StateUpdate());
        //SetSeason(GameManager.Instance.NowTime);
        WeatherState.WeatherListInit(EnviromentManager.Instance.Season);
        InitTempleats();

        GameObject firstTemp = Instantiate(firstObjectTemplates[0],this.transform);
        GameObject scecondTemp = Instantiate(secondObjectTemplates[0],this.transform);
        GameObject thridTemp = Instantiate(thirdObjectTemplates[0], this.transform);
        GameObject fourthTemp = Instantiate(fourthObjectTemplates[0], this.transform);

        Obstacles.AddRange(firstTemp.GetComponentsInChildren<BaseObstacle>());
        Obstacles.AddRange(scecondTemp.GetComponentsInChildren<BaseObstacle>());
        Obstacles.AddRange(thridTemp.GetComponentsInChildren<BaseObstacle>());
        Obstacles.AddRange(fourthTemp.GetComponentsInChildren<BaseObstacle>());

        foreach(BaseObstacle obstacle in Obstacles)
        {
            obstacle.Init(Season);
        }

        WheaterInit();
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

    }

   
    IEnumerator StateUpdate()
    {
        while (true)
        {
            WeatherState.Update();
            yield return waitForSeconds;
        }
        
    }

    private void InitTempleats()
    {
        firstObjectTemplates = Resources.LoadAll<GameObject>("Enviroment/First").ToList();
        secondObjectTemplates = Resources.LoadAll<GameObject>("Enviroment/Second").ToList();
        thirdObjectTemplates = Resources.LoadAll<GameObject>("Enviroment/Third").ToList();
        fourthObjectTemplates = Resources.LoadAll<GameObject>("Enviroment/Fourth").ToList();

        MapPrefabs.Add(Resources.Load<GameObject>("Enviroment/Maps/MapBase"));
        MapPrefabs.Add(Resources.Load<GameObject>("Enviroment/Maps/MapWinter"));

        Utils.Shuffle(firstObjectTemplates);
        Utils.Shuffle(secondObjectTemplates);
        Utils.Shuffle(thirdObjectTemplates);
        Utils.Shuffle(fourthObjectTemplates);
    }

    private void WheaterInit()
    {
        foreach (ParticleSystem particle in Resources.LoadAll<ParticleSystem>("Enviroment/Particle").ToList())
        {
            ParticleSystem particleSystem = Instantiate(particle, this.transform);
            Particles.Add(particleSystem);
            particleSystem.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        StopCoroutine(stateCorutine);
        stateCorutine = null;
    }
}
