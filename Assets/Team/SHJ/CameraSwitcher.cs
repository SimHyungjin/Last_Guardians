using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineVirtualCamera freeCam;
    [SerializeField] private CinemachineVirtualCamera focusCam;

    [SerializeField] private CinemachineConfiner2D freeCamBoundary;

    private bool isFocused = false;

    private void Start()
    {
        focusCam.Follow = InGameManager.Instance.playerManager.playerController.gameObject.transform;
        focusCam.Priority = 0;
        freeCam.Priority = 10;
    }

    public void ToggleFocus()
    {
        isFocused = !isFocused;

        if (isFocused)
        {
            focusCam.Priority = 10;
            freeCam.Priority = 0;
        }
        else
        {
            focusCam.Priority = 0;
            freeCam.Priority = 10;

            freeCamBoundary.InvalidateCache();
        }
    }

}
