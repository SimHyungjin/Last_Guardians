using UnityEngine;
using UnityEngine.EventSystems;

public class BlockerClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {

        InfoSidePanelUI.Instance.Hide();
        InGameCombinationUI.Instance.HideAndReset();
    }
}
