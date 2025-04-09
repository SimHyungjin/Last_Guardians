using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public FadeManager fadeManager;
    public void OnClickStart()
    {
        fadeManager.sceneToLoad = "GWGameSceneUI";
        fadeManager.StartFadeAndLoad();
    }
}



