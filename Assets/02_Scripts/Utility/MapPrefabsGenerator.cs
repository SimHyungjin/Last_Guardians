using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
    public List<GameObject> mapPrefabs;
    public List<Vector2> firstTilePositions=
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

  
    public List<Vector2> fourthTilePositions=
        new List<Vector2>
        {
            new Vector2(-8.5f, -0.5f), new Vector2(-7.5f, -0.5f), new Vector2(-6.5f, -0.5f), new Vector2(-5.5f, -0.5f), new Vector2(-4.5f, -0.5f), new Vector2(-3.5f, -0.5f), new Vector2(-2.5f, -0.5f),
            new Vector2(-9.5f, -1.5f), new Vector2(-8.5f, -1.5f), new Vector2(-7.5f, -1.5f), new Vector2(-6.5f, -1.5f), new Vector2(-5.5f, -1.5f), new Vector2(-4.5f, -1.5f), new Vector2(-3.5f, -1.5f), new Vector2(-2.5f, -1.5f),
            new Vector2(-8.5f, -2.5f), new Vector2(-7.5f, -2.5f), new Vector2(-6.5f, -2.5f), new Vector2(-5.5f, -2.5f), new Vector2(-4.5f, -2.5f), new Vector2(-3.5f, -2.5f), new Vector2(-2.5f, -2.5f), new Vector2(-1.5f, -2.5f),
            new Vector2(-7.5f, -3.5f), new Vector2(-6.5f, -3.5f), new Vector2(-5.5f, -3.5f), new Vector2(-4.5f, -3.5f), new Vector2(-3.5f, -3.5f), new Vector2(-2.5f, -3.5f), new Vector2(-1.5f, -3.5f),
            new Vector2(-6.5f, -4.5f), new Vector2(-5.5f, -4.5f), new Vector2(-4.5f, -4.5f), new Vector2(-3.5f, -4.5f), new Vector2(-2.5f, -4.5f), new Vector2(-1.5f, -4.5f),
        };
    public void StartGenerate(string inputCode)
    {
        Debug.Log("Start Generate");
        Debug.Log(inputCode);
    }
}
