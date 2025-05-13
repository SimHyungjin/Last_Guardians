using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerUpgradeButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button clickedButton;
    private bool isButtonHeld = false;

    public delegate void ButtonHeldDelegate(Button button,bool isHeld);
    public event ButtonHeldDelegate OnButtonHeld;
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        clickedButton = clickedObject.GetComponent<Button>();
        if (clickedButton == null)
        {
            return;
        }
        if (clickedButton.interactable == true)
        {
            isButtonHeld=true;
            OnButtonHeld?.Invoke(clickedButton, isButtonHeld);
        }
        else
        {
            clickedButton = null;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (clickedButton == null)
        {
            return;
        }
        else 
        {
            isButtonHeld = false;
            OnButtonHeld?.Invoke(clickedButton, isButtonHeld);
            clickedButton = null;
        }

    }
}
