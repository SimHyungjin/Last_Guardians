using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputToggleUI : MonoBehaviour
{
    [SerializeField] private JoystickUIController joystickUIController;
    public RectTransform handle;
    public RectTransform slideArea;
    public GameObject slideAreaSecond;
    public Button toggleButton;

    private PlayerInputStyle CurrentMode;

    private void Start()
    {
        CurrentMode = GameManager.Instance.PlayerManager.playerInputStyle;
        toggleButton.onClick.AddListener(OnToggleClicked);
        SetMode(CurrentMode);
    }
    private void OnDestroy()
    {
        GameManager.Instance.PlayerManager.ChangeInputStyle(CurrentMode);
        SaveSystem.SaveInputStyle(CurrentMode);
    }
    private void OnToggleClicked()
    {
        var nextMode = (CurrentMode == PlayerInputStyle.Swipe) ? PlayerInputStyle.Joystick : PlayerInputStyle.Swipe;
        GameManager.Instance.PlayerManager.ChangeInputStyle(nextMode);
        SetMode(nextMode);
    }

    private void SetMode(PlayerInputStyle mode)
    {
        CurrentMode = mode;

        float halfWidth = slideArea.rect.width * 0.4f;
        float x = (mode == PlayerInputStyle.Swipe) ? -halfWidth : halfWidth;

        handle.anchoredPosition = new Vector2(x, handle.anchoredPosition.y);
        slideAreaSecond.SetActive(CurrentMode != PlayerInputStyle.Swipe);
        if (joystickUIController != null) joystickUIController.SetVisible(CurrentMode);
    }
}
