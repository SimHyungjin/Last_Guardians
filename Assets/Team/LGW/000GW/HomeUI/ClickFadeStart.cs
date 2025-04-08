using UnityEngine;
using UnityEngine.EventSystems;

public class ClickFadeStart : MonoBehaviour, IPointerClickHandler
{
    public FadeManager fadeManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        fadeManager.StartFadeAndLoad();
    }
}

