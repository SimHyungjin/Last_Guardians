using UnityEngine;

public class ExitButton : MonoBehaviour
{
    //public FadeManager fadeManager;
    public void OnClickExit()
    {
        InGameManager.Instance.GameExit();
        Time.timeScale = 0f;
        //SceneLoader.LoadSceneWithFade("MainScene", true);
    }

}
