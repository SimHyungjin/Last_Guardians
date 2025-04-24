#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(MapPrefabsGenerator))]
public class MapPrefabsGeneratorEditor : Editor
{
    string inputCode = "1000000001066005551106600000000000200"; // 기본값

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapPrefabsGenerator fnc = (MapPrefabsGenerator)target;

        GUILayout.Space(10);
        GUILayout.Label("장애물 배치 시드 입력 (37자리)", EditorStyles.boldLabel);
        inputCode = EditorGUILayout.TextField("Seed Code", inputCode);

        if (inputCode.Length != 37)
        {
            EditorGUILayout.HelpBox("시드는 37자리 숫자로 입력해야 합니다.", MessageType.Warning);
        }

        if (GUILayout.Button("Start Generate"))
        {
            if (inputCode.Length == 37)
            {
                fnc.StartGenerate(inputCode);
            }
            else
            {
                Debug.LogError("시드는 반드시 37자리여야 합니다.");
            }
        }
    }
}
public class MapPrefabsGenerator : MonoBehaviour
{
    public List<Vector2> firstTilePositions =
        new List<Vector2>
        {
            new Vector2(-6.5f, 3.5f), new Vector2(-5.5f, 3.5f), new Vector2(-4.5f, 3.5f), new Vector2(-3.5f, 3.5f), new Vector2(-2.5f, 3.5f), new Vector2(-1.5f, 3.5f), new Vector2(-0.5f, 3.5f),new Vector2(0.5f, 3.5f),
            new Vector2(-7.5f, 2.5f), new Vector2(-6.5f, 2.5f), new Vector2(-5.5f, 2.5f), new Vector2(-4.5f, 2.5f), new Vector2(-3.5f, 2.5f), new Vector2(-2.5f, 2.5f), new Vector2(-1.5f, 2.5f), new Vector2(-0.5f, 2.5f), new Vector2(0.5f, 2.5f),
            new Vector2(-8.5f, 1.5f), new Vector2(-7.5f, 1.5f), new Vector2(-6.5f, 1.5f), new Vector2(-5.5f, 1.5f), new Vector2(-4.5f, 1.5f), new Vector2(-3.5f, 1.5f), new Vector2(-2.5f, 1.5f), new Vector2(-1.5f, 1.5f),new Vector2(-0.5f,1.5f), new Vector2(0.5f, 1.5f),
            new Vector2(-9.5f, 0.5f), new Vector2(-8.5f, 0.5f), new Vector2(-7.5f, 0.5f), new Vector2(-6.5f, 0.5f), new Vector2(-5.5f, 0.5f), new Vector2(-4.5f, 0.5f), new Vector2(-3.5f, 0.5f), new Vector2(-2.5f, 0.5f),
            new Vector2(-9.5f, -0.5f)
        };
    public List<Vector2> secondTilePositions =
        new List<Vector2>
        {
            new Vector2(1.5f, 3.5f), new Vector2(2.5f, 3.5f), new Vector2(3.5f, 3.5f), new Vector2(4.5f, 3.5f), new Vector2(5.5f, 3.5f), new Vector2(6.5f, 3.5f),
            new Vector2(1.5f, 2.5f), new Vector2(2.5f, 2.5f), new Vector2(3.5f, 2.5f), new Vector2(4.5f, 2.5f), new Vector2(5.5f, 2.5f), new Vector2(6.5f, 2.5f), new Vector2(7.5f, 2.5f),
            new Vector2(1.5f, 1.5f), new Vector2(2.5f, 1.5f), new Vector2(3.5f, 1.5f), new Vector2(4.5f, 1.5f), new Vector2(5.5f, 1.5f), new Vector2(6.5f, 1.5f), new Vector2(7.5f, 1.5f),new Vector2(8.5f, 1.5f),
            new Vector2(2.5f, 0.5f), new Vector2(3.5f, 0.5f), new Vector2(4.5f, 0.5f), new Vector2(5.5f, 0.5f), new Vector2(6.5f, 0.5f), new Vector2(7.5f, 0.5f), new Vector2(8.5f, 0.5f),new Vector2(9.5f, 0.5f),
            new Vector2(2.5f, -0.5f), new Vector2(3.5f, -0.5f), new Vector2(4.5f, -0.5f), new Vector2(5.5f, -0.5f), new Vector2(6.5f, -0.5f), new Vector2(7.5f, -0.5f), new Vector2(8.5f, -0.5f)
        };
    public List<Vector2> thirdTilePositions =
        new List<Vector2>
        {
            new Vector2(9.5f, -0.5f),
            new Vector2(2.5f, -1.5f), new Vector2(3.5f, -1.5f), new Vector2(4.5f, -1.5f), new Vector2(5.5f, -1.5f), new Vector2(6.5f, -1.5f), new Vector2(7.5f, -1.5f), new Vector2(8.5f, -1.5f),new Vector2(9.5f, -1.5f),
            new Vector2(-0.5f, -2.5f), new Vector2 (0.5f, -2.5f), new Vector2(1.5f, -2.5f), new Vector2(2.5f, -2.5f), new Vector2(3.5f, -2.5f), new Vector2(4.5f, -2.5f), new Vector2(5.5f, -2.5f), new Vector2(6.5f, -2.5f), new Vector2(7.5f, -2.5f),new Vector2(8.5f, -2.5f),
            new Vector2(-0.5f, -3.5f), new Vector2 (0.5f, -3.5f), new Vector2(1.5f, -3.5f), new Vector2(2.5f, -3.5f), new Vector2(3.5f, -3.5f), new Vector2(4.5f, -3.5f), new Vector2(5.5f, -3.5f), new Vector2(6.5f, -3.5f), new Vector2(7.5f, -3.5f),
            new Vector2(-0.5f, -4.5f), new Vector2 (0.5f, -4.5f), new Vector2(1.5f, -4.5f), new Vector2(2.5f, -4.5f), new Vector2(3.5f, -4.5f), new Vector2(4.5f, -4.5f), new Vector2(5.5f, -4.5f), new Vector2(6.5f, -4.5f),
        };


    public List<Vector2> fourthTilePositions =
        new List<Vector2>
        {
            new Vector2(-8.5f, -0.5f), new Vector2(-7.5f, -0.5f), new Vector2(-6.5f, -0.5f), new Vector2(-5.5f, -0.5f), new Vector2(-4.5f, -0.5f), new Vector2(-3.5f, -0.5f), new Vector2(-2.5f, -0.5f),
            new Vector2(-9.5f, -1.5f), new Vector2(-8.5f, -1.5f), new Vector2(-7.5f, -1.5f), new Vector2(-6.5f, -1.5f), new Vector2(-5.5f, -1.5f), new Vector2(-4.5f, -1.5f), new Vector2(-3.5f, -1.5f), new Vector2(-2.5f, -1.5f),
            new Vector2(-8.5f, -2.5f), new Vector2(-7.5f, -2.5f), new Vector2(-6.5f, -2.5f), new Vector2(-5.5f, -2.5f), new Vector2(-4.5f, -2.5f), new Vector2(-3.5f, -2.5f), new Vector2(-2.5f, -2.5f), new Vector2(-1.5f, -2.5f),
            new Vector2(-7.5f, -3.5f), new Vector2(-6.5f, -3.5f), new Vector2(-5.5f, -3.5f), new Vector2(-4.5f, -3.5f), new Vector2(-3.5f, -3.5f), new Vector2(-2.5f, -3.5f), new Vector2(-1.5f, -3.5f),
            new Vector2(-6.5f, -4.5f), new Vector2(-5.5f, -4.5f), new Vector2(-4.5f, -4.5f), new Vector2(-3.5f, -4.5f), new Vector2(-2.5f, -4.5f), new Vector2(-1.5f, -4.5f),
        };

    public GameObject ObstaclePrefab;
    public void StartGenerate(string code)
    {
        if (code.Length != 37)
        {
            Debug.LogError("코드는 반드시 37자리여야 합니다.");
            return;
        }

        int areaIndex = code[0] - '1';
        List<Vector2> positions = areaIndex switch
        {
            0 => firstTilePositions,
            1 => secondTilePositions,
            2 => thirdTilePositions,
            3 => fourthTilePositions,
            _ => null
        };

        string areaName = areaIndex switch
        {
            0 => "first",
            1 => "second",
            2 => "third",
            3 => "fourth",
            _ => "unknown"
        };

        if (positions == null || positions.Count != 36)
        {
            Debug.LogError("잘못된 구역이거나 좌표 개수가 36개가 아닙니다.");
            return;
        }

        GameObject root = new GameObject($"{areaName}ObstacleTemplat");

        for (int i = 0; i < 36; i++)
        {
            int prefabCode = code[i + 1] - '0';
            if (prefabCode == 0) continue;

            int enumIndex = prefabCode - 1;
            if (enumIndex < 0 || enumIndex >= 7) continue;

            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(ObstaclePrefab);
            obj.transform.position = positions[i];
            obj.transform.SetParent(root.transform);

            var obstacleComponent = obj.GetComponent<BaseObstacle>();
            if (obstacleComponent != null)
            {
                //obstacleComponent.Init((ObstacleType)enumIndex);
            }
            else
            {
                Debug.LogWarning("ObstacleComponent가 없습니다.");
            }
        }

        string folderPath = "Assets/03_prefabs/ObstacleTemplat";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
        }

        string fileName = $"{areaName}ObstacleTemplat";
        string savePath = GetUniquePrefabPath(folderPath, fileName);
        PrefabUtility.SaveAsPrefabAssetAndConnect(root, savePath, InteractionMode.UserAction);
        DestroyImmediate(root);
    }

    string GetUniquePrefabPath(string basePath, string fileName)
    {
        string path = $"{basePath}/{fileName}.prefab";
        int counter = 1;

        while (File.Exists(path))
        {
            path = $"{basePath}/{fileName}({counter}).prefab";
            counter++;
        }

        return path;
    }
}
#endif