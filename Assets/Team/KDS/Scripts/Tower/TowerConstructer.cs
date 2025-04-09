using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerConstructer : MonoBehaviour
{   
    private TowerCombinationData towerCombinationData;
    public GameObject towerPrefab;
    private void Awake()
    {
        towerCombinationData = TowerManager.Instance.towerCombinationData;
    }
    public bool CanCombine(int ing1, int ing2)
    {
        int result = towerCombinationData.TryCombine(ing1, ing2);
        if (result == -1) return false;
        return true;
    }
    public bool CanConstruct(Vector2 curPos)
    {
        Vector2 constructPos = PostionArray(curPos);
        if (IsAnyObjectOnTile(constructPos))
        {
            Debug.Log("무언가가 이미 이 자리에 있음");
            return false;
        }
        return true;
    }

    public void TowerConstruct(Vector2 curPos,int TowerIndex)
    {
        Vector2 constructPos = PostionArray(curPos);
        GameObject go = Instantiate(towerPrefab, constructPos, Quaternion.identity);
        Tower tower = go.GetComponent<Tower>();
        tower.Init(TowerIndex);
    }

    public void TowerCombine(Tower ing1, Tower ing2)
    {
        int result = towerCombinationData.TryCombine(ing1.towerdata.TowerIndex, ing2.towerdata.TowerIndex);
        if (result == -1) return;
        Vector2 combinePos = InputManager.Instance.GetTouchWorldPosition();
        Vector2 constructPos = PostionArray(combinePos);
        Destroy(ing1.gameObject);
        Destroy(ing2.gameObject);
    }

    public bool IsAnyObjectOnTile(Vector2 tilePos)
    {
        Collider2D hit = Physics2D.OverlapPoint(tilePos);
        return hit != null;
    }
    public Vector2 PostionArray(Vector2 pos)
    {
        return new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }
}
