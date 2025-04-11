using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public FadeManager fadeManager;
    public void OnClickExit()
    {
        Time.timeScale = 1;
        fadeManager.sceneToLoad = "00GWUI";
        fadeManager.StartFadeAndLoad();

    }

}
