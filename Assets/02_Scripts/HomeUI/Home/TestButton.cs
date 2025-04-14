using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    [SerializeField] private CameraSwitcher virtualCamera;
    [Header("카메라 테스트 버튼")]
    [SerializeField] private Button zoomBtn;
    [SerializeField] private Button timeScaleBtn;

    bool check = false;

    private void Start()
    {
        if(zoomBtn != null)
            zoomBtn.onClick.AddListener(ZoomTest);

        //timeScaleBtn.onClick.AddListener(TimeScale);
    }
    private void ZoomTest()
    {
        virtualCamera.ToggleFocus();
    }


    private void TimeScale()
    {
        check = !check;

        if (!check)
        {

            InputManager.Instance.DisablePointer(); Time.timeScale = 0;
        }
        else
        {

            InputManager.Instance.EnablePointer();
            Time.timeScale = 2f;
        }
    }
}