using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptionUI : MonoBehaviour
{
    public GameObject optionPanel;

    public void OpenOption()
    {
        optionPanel.SetActive(true);
    }
    public void CloseOption()
    {
        optionPanel.SetActive(false);
    }
}
