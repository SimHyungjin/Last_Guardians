using UnityEditor;
using UnityEngine;

public class FindMissingScriptsInProject
{
    [MenuItem("Tools/Find Missing Scripts in Project")]
    public static void FindMissingScripts()
    {
        int missingCount = 0;
        string[] allPrefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });

        foreach (string guid in allPrefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            Component[] components = prefab.GetComponentsInChildren<Component>(true);
            foreach (Component c in components)
            {
                if (c == null)
                {
                    Debug.LogWarning($"Missing script in prefab: {path}", prefab);
                    missingCount++;
                }
            }
        }

        Debug.Log($"프로젝트 전체 검색 완료. Missing Script {missingCount}개 발견됨.");
    }
}
