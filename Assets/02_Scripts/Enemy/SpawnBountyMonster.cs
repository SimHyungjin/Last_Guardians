using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBountyMonster : MonoBehaviour
{
    private Button spawnBtn;

    private void Awake()
    {
        spawnBtn = GetComponent<Button>();
    }
    private void Start()
    {
        spawnBtn.onClick.AddListener(SpwanBountyMonster);
    }

    private void SpwanBountyMonster()
    {
        if(MonsterManager.Instance != null)
        {
            MonsterManager.Instance.SpawnBounty();
        }
        
    }
}
