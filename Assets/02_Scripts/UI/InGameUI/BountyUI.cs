using UnityEngine;
using UnityEngine.UI;

public class BountyUI : MonoBehaviour
{
    [SerializeField] private GameObject bountyPanel;
    private Button openUIBtn;

    private void Awake()
    {
        openUIBtn = GetComponent<Button>();
    }

    private void Start()
    {
        openUIBtn.onClick.AddListener(OpenBountyUI);
    }

    public void OpenBountyUI()
    {
        if (bountyPanel.gameObject.activeSelf)
        {
            bountyPanel.SetActive(false);
        }
        else
            bountyPanel.SetActive(true);
    }
}
