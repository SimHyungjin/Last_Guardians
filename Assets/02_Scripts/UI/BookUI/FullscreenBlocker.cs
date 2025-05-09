using UnityEngine;
using UnityEngine.EventSystems;

public class FullscreenBlocker : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        
        GameObject clicked = eventData.pointerCurrentRaycast.gameObject;

        
        if (clicked != null && clicked.GetComponent<TowerSlot>() != null)
        {
            return;
        }

        
        TowerCombinationUI.Instance.HidePanel();
    }
}
