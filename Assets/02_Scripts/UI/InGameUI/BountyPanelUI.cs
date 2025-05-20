using UnityEngine;

public class BountyPanelUI : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private SpawnBountyMonster bountyMonsterBtn;

    private void Awake()
    {
        for (int i = 0; i < MonsterManager.Instance.BountyMonsterList.Count; i++)
        {
            SpawnBountyMonster btn = Instantiate(bountyMonsterBtn, content);
            btn.Init(MonsterManager.Instance.BountyMonsterList[i]);
        }
    }
}
