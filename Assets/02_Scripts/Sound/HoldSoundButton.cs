using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class HoldSoundButton : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerExitHandler
{
    [Tooltip("누르고 있는 동안 반복 재생할 효과음 이름")]
    public string holdSoundName = "Upgrade";

    private bool isHeld = false;

    // 버튼을 누르는 순간마다 호출
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"HoldSoundButton: OnPointerDown on {gameObject.name}");
        isHeld = true;
        SoundManager.Instance.PlaySFXLoop(holdSoundName);
    }

    // 버튼에서 손을 뗄 때
    public void OnPointerUp(PointerEventData eventData)
    {
        ReleaseHold();
    }

    // 버튼 영역 밖으로 드래그되어 나갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        ReleaseHold();
    }

    private void ReleaseHold()
    {
        if (!isHeld) return;
        isHeld = false;
       
    }
}
