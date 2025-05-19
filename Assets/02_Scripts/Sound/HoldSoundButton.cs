using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class HoldSoundButton : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerExitHandler
{
    [Tooltip("������ �ִ� ���� �ݺ� ����� ȿ���� �̸�")]
    public string holdSoundName = "Upgrade";

    private bool isHeld = false;

    // ��ư�� ������ �������� ȣ��
    public void OnPointerDown(PointerEventData eventData)
    {
        
        isHeld = true;
        SoundManager.Instance.PlaySFXLoop(holdSoundName);
    }

    // ��ư���� ���� �� ��
    public void OnPointerUp(PointerEventData eventData)
    {
        ReleaseHold();
    }

    // ��ư ���� ������ �巡�׵Ǿ� ������ ��
    public void OnPointerExit(PointerEventData eventData)
    {
        ReleaseHold();
    }

    private void ReleaseHold()
    {
        if (!isHeld) return;
        isHeld = false;

        // ���⼭ ���� ����� �ݵ�� ����
        SoundManager.Instance.StopSFX();

        
    }
}
