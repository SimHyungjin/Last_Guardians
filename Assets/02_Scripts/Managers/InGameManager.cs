using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public PlayerManager playerManager { get; private set; }

    public int level;
    public int exp;
    public int maxExp;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        playerManager = new();
        playerManager.Init();
    }

    public void GetExp(int exp)
    {
        this.exp += exp;
        if (this.exp >= maxExp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        level++;
        exp = 0;
    }
}
