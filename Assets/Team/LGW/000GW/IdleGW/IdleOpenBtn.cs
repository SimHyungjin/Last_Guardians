using UnityEngine;
using UnityEngine.UI;

public class IdleOpenBtn : MonoBehaviour
{
    [SerializeField] private IdleRewardPopup popup;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            popup.OpenPopup();
        });
    }
}
