using UnityEngine;
using UnityEngine.UI;

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
