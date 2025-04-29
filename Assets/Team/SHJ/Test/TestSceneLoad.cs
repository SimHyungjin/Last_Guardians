using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneLoad : MonoBehaviour
{
    public Button sceneLoad;

    private void Awake()
    {
        sceneLoad.onClick.AddListener(TestSceneLoader);
    }
    public void TestSceneLoader()
    {
        SceneLoader.LoadScene("GameScene", BeforeSceneLoader);
    }

    private void BeforeSceneLoader()
    {
        SaveSystem.SaveGame();
        GameManager.Instance.stats = MainSceneManager.Instance.equipment.InfoToPlayer();
    }
}
