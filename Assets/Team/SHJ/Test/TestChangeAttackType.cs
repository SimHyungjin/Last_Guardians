using UnityEngine;
using UnityEngine.UI;

public class TestChangeAttackType : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonParent;
    public AttackController attackController;

    void Start()
    {
        attackController = FindObjectOfType<AttackController>();
        CreateAttackButton("근거리 정면", () => attackController.SetAttackBehavior(new AttackMeleeFront()));
        CreateAttackButton("근거리 원형", () => attackController.SetAttackBehavior(new AttackMeleeCircle()));
        CreateAttackButton("원거리 단일", () => attackController.SetAttackBehavior(new AttackRangedSingle()));
        CreateAttackButton("원거리 범위", () => attackController.SetAttackBehavior(new AttackRangedMulti()));
    }

    void CreateAttackButton(string label, UnityEngine.Events.UnityAction onClickAction)
    {
        var btnObj = Instantiate(buttonPrefab, buttonParent);
        var btn = btnObj.GetComponent<Button>();
        var txt = btnObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (txt != null) txt.text = label;

        btn.onClick.AddListener(onClickAction);
    }
}