using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public PlayerManager playerManager {  get; private set; }

    public int level;
    public int exp;
    public int maxExp;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    void Init()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if(currentScene == "Character_SHJ_Sene")
        {
            playerManager = new();
            playerManager.Init();
        }
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
