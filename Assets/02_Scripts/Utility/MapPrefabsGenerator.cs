using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapPrefabsGenerator))]
public class MapPrefabsGeneratorEditor : Editor
{
    string inputCode = "1000000001066005551106600000000000200"; // �⺻��

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapPrefabsGenerator fnc = (MapPrefabsGenerator)target;

        GUILayout.Space(10);
        GUILayout.Label("��ֹ� ��ġ �õ� �Է� (37�ڸ�)", EditorStyles.boldLabel);
        inputCode = EditorGUILayout.TextField("Seed Code", inputCode);

        if (inputCode.Length != 37)
        {
            EditorGUILayout.HelpBox("�õ�� 37�ڸ� ���ڷ� �Է��ؾ� �մϴ�.", MessageType.Warning);
        }

        if (GUILayout.Button("Start Generate"))
        {
            if (inputCode.Length == 37)
            {
                fnc.StartGenerate(inputCode);
            }
            else
            {
                Debug.LogError("�õ�� �ݵ�� 37�ڸ����� �մϴ�.");
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
