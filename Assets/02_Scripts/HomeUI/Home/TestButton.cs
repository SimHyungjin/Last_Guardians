using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    [SerializeField] private CameraSwitcher virtualCamera;
    [Header("카메라 테스트 버튼")]
    [SerializeField] private Button zoomBtn;

    private void Start()
    {
        if(zoomBtn != null)
            zoomBtn.onClick.AddListener(ZoomTest);
    }
    private void ZoomTest()
    {
        virtualCamera.ToggleFocus();
    }

}