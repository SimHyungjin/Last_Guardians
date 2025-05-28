// TowerUpgradeButtonHandler.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TowerUpgradeButtonHandler : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerExitHandler
{
    [Tooltip("누르고 있는 동안 반복 재생할 효과음 이름")]
    public string holdSoundName = "Upgrade";

    private Button clickedButton;
    private bool isButtonHeld = false;

    public delegate void ButtonHeldDelegate(Button button, bool isHeld);
    public event ButtonHeldDelegate OnButtonHeld;

    public void OnPointerDown(PointerEventData eventData)
    {
        var go = eventData.pointerCurrentRaycast.gameObject;
        clickedButton = go != null ? go.GetComponent<Button>() : null;
        if (clickedButton != null && clickedButton.interactable)
        {
            isButtonHeld = true;

            SoundManager.Instance.PlaySFXLoop(holdSoundName);

            OnButtonHeld?.Invoke(clickedButton, isButtonHeld);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ReleaseHold();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ReleaseHold();
    }

    private void ReleaseHold()
    {
        if (!isButtonHeld) return;
        isButtonHeld = false;


        OnButtonHeld?.Invoke(clickedButton, isButtonHeld);
        clickedButton = null;
    }
}
