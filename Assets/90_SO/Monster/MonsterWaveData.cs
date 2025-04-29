using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveData", menuName = "Data/Wave Data")]
public class MonsterWaveData : ScriptableObject
{
    [SerializeField] private int waveIndex;
    [SerializeField] private int waveLevel;
    [SerializeField] private bool isBoss;
    [SerializeField] private float waveStartDelay;
    [SerializeField] private float monsterSpawnInterval;
    [SerializeField] private int monster1ID;
    [SerializeField] private int monster1Value;
    [SerializeField] private int monster2ID;
    [SerializeField] private int monster2Value;
    [SerializeField] private int monster3ID;
    [SerializeField] private int monster3Value;
    [SerializeField] private int monster4ID;
    [SerializeField] private int monster4Value;
    [SerializeField] private float bossMultiplier;

    public int WaveIndex => waveIndex;
    public int WaveLevel => waveLevel;
    public bool IsBoss => isBoss;
    public float WaveStartDelay => waveStartDelay;
    public float MonsterSpawnInterval => monsterSpawnInterval;
    public int Monster1ID => monster1ID;
    public int Monster1Value => monster1Value;
    public int Monster2ID => monster2ID;
    public int Monster2Value => monster2Value;
    public int Monster3ID => monster3ID;
    public int Monster3Value => monster3Value;
    public int Monster4ID => monster4ID;
    public int Monster4Value => monster4Value;
    public float BossMultiplier => bossMultiplier;

    public void SetData(int waveIndex, int waveLevel, bool isBoss, float waveStartDelay, float monsterSpawnInterval, int monster1ID, int monster1Value, int monster2ID, int monster2Value, int monster3ID, int monster3Value, int monster4ID, int monster4Value, float bossMultiplier)
    {
        this.waveIndex = waveIndex;
        this.waveLevel = waveLevel;
        this.isBoss = isBoss;
        this.waveStartDelay = waveStartDelay;
        this.monsterSpawnInterval = monsterSpawnInterval;
        this.monster1ID = monster1ID;
        this.monster1Value = monster1Value;
        this.monster2ID = monster2ID;
        this.monster2Value = monster2Value;
        this.monster3ID = monster3ID;
        this.monster3Value = monster3Value;
        this.monster4ID = monster4ID;
        this.monster4Value = monster4Value;
        this.bossMultiplier = bossMultiplier;
    }
}
