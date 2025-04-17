using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBountyMonster : MonoBehaviour
{
    private Button spawnBtn;
    private Image spawnImage;
    private TextMeshProUGUI spawnText;
    private int spawnIndex;

    private void Awake()
    {
        spawnBtn = GetComponent<Button>();
        spawnImage = GetComponentInChildren<Image>();
        spawnText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        spawnBtn.onClick.AddListener(SpwanBountyMonster);
    }

    public void Init(MonsterData data)
    {
        spawnImage.sprite = data.Image;
        spawnText.text = data.MonsterName;
        spawnIndex = data.MonsterIndex;
    }

    private void SpwanBountyMonster()
    {
        if(MonsterManager.Instance != null && MonsterManager.Instance.SpawnTimer == 0f)
        {
            MonsterManager.Instance.SpawnBounty(spawnIndex);
            MonsterManager.Instance.SpawnTimer = MonsterManager.Instance.BountySpwanCoolTime;
        }
        
    }
}
