using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PopupCloseButton은 팝업을 닫는 버튼을 관리하는 클래스입니다.
/// </summary>
public class PopupCloseButton : MonoBehaviour
{
    private MainSceneManager mainSceneManager;
    private Button button;

    private void Awake()
    {
        mainSceneManager = MainSceneManager.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickClose);
    }
    /// <summary>
    /// 팝업을 닫는 메서드입니다.
    /// </summary>
    public void OnClickClose()
    {
        var blocker = mainSceneManager.interactionBlocker.transform;
        var parent = blocker.parent;
        int panelIndex = blocker.GetSiblingIndex() + 1;
        if (panelIndex >= parent.childCount) return;
        var panel = parent.GetChild(panelIndex).gameObject;
        if (panel == null) return;
        mainSceneManager.HidePanel(panel.name);
    }
}
