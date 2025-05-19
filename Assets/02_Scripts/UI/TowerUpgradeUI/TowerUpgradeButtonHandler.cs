// TowerUpgradeButtonHandler.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class TowerUpgradeButtonHandler : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerExitHandler
{
    [Tooltip("������ �ִ� ���� �ݺ� ����� ȿ���� �̸�")]
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
