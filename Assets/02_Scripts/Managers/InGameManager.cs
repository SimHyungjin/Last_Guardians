using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public PlayerManager playerManager {  get; private set; }

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
}
