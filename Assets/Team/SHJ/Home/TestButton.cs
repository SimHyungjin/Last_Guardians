using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private CameraSwitcher virtualCamera;

    [Header("장비 타입별 테스트 버튼")]
    [SerializeField] private Button weaponButton;
    [SerializeField] private Button armorButton;
    [SerializeField] private Button helmetButton;
    [SerializeField] private Button ringButton;
    [SerializeField] private Button shoesButton;
    [SerializeField] private Button ALLButton;

    [Header("카메라 테스트 버튼")]
    [SerializeField] private Button zoomBtn;


    private void Start()
    {
        if (weaponButton != null)
            weaponButton.onClick.AddListener(() => SetType(ItemType.Weapon));

        if (armorButton != null)
            armorButton.onClick.AddListener(() => SetType(ItemType.Armor));

        if (helmetButton != null)
            helmetButton.onClick.AddListener(() => SetType(ItemType.Helmet));

        if (ringButton != null)
            ringButton.onClick.AddListener(() => SetType(ItemType.Ring));

        if (shoesButton != null)
            shoesButton.onClick.AddListener(() => SetType(ItemType.Shoes));

        if (ALLButton != null)
            ALLButton.onClick.AddListener(() => SetType(ItemType.Count));

        if(zoomBtn != null)
            zoomBtn.onClick.AddListener(ZoomTest);


    }
    private void ZoomTest()
    {
        virtualCamera.ToggleFocus();
    }
    private void SetType(ItemType type)
    {
        inventory.SetInventoryType(type);
        Debug.Log($"[Test] 장비 타입 전환: {type}");
    }
}