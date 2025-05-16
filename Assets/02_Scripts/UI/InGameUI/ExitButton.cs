using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

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
