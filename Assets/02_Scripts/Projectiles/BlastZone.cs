using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlastZone : MonoBehaviour
{
    private TowerData towerData;
    private Transform blastpos;
    public void Init(TowerData towerData,Transform _blastpos)
    {
#if UNITY_EDITOR
        string spritename = $"{towerData.ElementType}BlastEffect";
        string path = $"Assets/91_Disign/Sprite/ProjectileImage/BlastEffects/{spritename}.png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        GetComponent<SpriteRenderer>().sprite = sprite;
#endif
    }
}
