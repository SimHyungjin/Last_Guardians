using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartSceneClick : MonoBehaviour, IPointerClickHandler
{
    public string sceneName = "00GWUI";

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(sceneName);
    }
}
