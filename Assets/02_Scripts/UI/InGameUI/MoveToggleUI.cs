using System;
using UnityEngine;
using UnityEngine.UI;

public class MoveToggleUI : MonoBehaviour
{
    public RectTransform handle;
    public RectTransform slideArea;
    public GameObject slideAreaSecond;
    public Button toggleButton;


    public InputMode CurrentMode { get; private set; }

    public Action<InputMode> OnModeChanged;

    private void Start()
    {
        toggleButton.onClick.AddListener(OnToggleClicked);
        SetMode(InputMode.Swipe);
    }

    private void OnToggleClicked()
    {
        var nextMode = (CurrentMode == InputMode.Swipe) ? InputMode.Joystick : InputMode.Swipe;
        SetMode(nextMode);
    }

    private void SetMode(InputMode mode)
    {
        CurrentMode = mode;

        float halfWidth = slideArea.rect.width * 0.5f;
        float x = (mode == InputMode.Swipe) ? -halfWidth : halfWidth;

        handle.anchoredPosition = new Vector2(x, handle.anchoredPosition.y);
        slideAreaSecond.SetActive(CurrentMode != InputMode.Swipe);
        OnModeChanged?.Invoke(mode);
    }
}
