using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineVirtualCamera freeCam;
    [SerializeField] private CinemachineVirtualCamera focusCam;

    private CinemachineConfiner2D freeCamBoundary;
    private CinemachineConfiner2D focusCamBoundary;

    [SerializeField] GameObject dummyObj;

    private bool isFocused = false;

    PlayerHandler playerController;

    private Coroutine focusCoroutine;

    private void Start()
    {
        freeCamBoundary = freeCam.GetComponent<CinemachineConfiner2D>();
        focusCamBoundary = focusCam.GetComponent<CinemachineConfiner2D>();
        playerController = GameManager.Instance.PlayerManager.playerHandler;
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
            focusCamBoundary.InvalidateCache();
            if(focusCoroutine != null)
            {
                StopCoroutine(focusCoroutine);
                focusCoroutine = null;
            }
            StartCoroutine(LerpMove());

        }
        else
        {
            focusCam.Priority = 0;
            freeCam.Priority = 10;
            dummyObj.transform.position = (Vector2)Camera.main.transform.position;
            freeCamBoundary.InvalidateCache();
        }
    }

    private IEnumerator LerpMove()
    {
        Vector3 startPos = dummyObj.transform.position;
        Vector3 targetPos = playerController.transform.position;
        targetPos.z = startPos.z;

        float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            dummyObj.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        dummyObj.transform.position = targetPos;
    }
}
