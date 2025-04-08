using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using UnityEngine.InputSystem;
using Unity.VisualScripting;


[CustomEditor(typeof(DataDownLoader))]
public class SheetDownButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DataDownLoader fnc = (DataDownLoader)target;
        if (GUILayout.Button("Download SheetData"))
        {
            fnc.StartDownload(true);
        }
        if (GUILayout.Button("Download CombinationData"))
        {
            fnc.StartCombinationDataDownload(true);
        }
    }
}


public class DataDownLoader : MonoBehaviour
{
    [SerializeField] private List<DataSO> abilityDataSO = new List<DataSO>();
    [SerializeField] public TowerCombinationData towerCombinationData;
    const string URL_DataSheet = "https://docs.google.com/spreadsheets/d/11uh3qkFXuMsowtu7qrmO66nYMx46C2P9UHh3c1eHVu4/export?format=tsv&range=A1:F6";


    public void StartDownload(bool renameFiles)
    {
        StartCoroutine(DownloadData(renameFiles));
    }
    IEnumerator DownloadData(bool renameFiles)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL_DataSheet);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string tsvText = www.downloadHandler.text;
            string json = ConvertTSVToJson(tsvText);

            JArray jsonData = JArray.Parse(json); // JSON 문자열을 JArray로 변환
            ApplyDataToSO(jsonData, renameFiles);
        }
        else
        {
            Debug.LogError("데이터 가져오기 실패: " + www.error);
        }
    }


    string ConvertTSVToJson(string tsv)
    {
        string[] lines = tsv.Split('\n'); // 줄 단위로 분리
        if (lines.Length < 2) return "[]"; // 데이터가 없으면 빈 JSON 배열 반환

        string[] headers = lines[0].Split('\t'); // 첫 번째 줄을 헤더로 사용
        JArray jsonArray = new JArray();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split('\t'); // 데이터 값 분리
            JObject jsonObject = new JObject();

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                string cleanValue = values[j].Trim();
                jsonObject[headers[j].Trim()] = cleanValue; // `+` 인코딩 안 함
            }

            jsonArray.Add(jsonObject);
        }

        return jsonArray.ToString();
    }

    string ConvertTSVToJson(string tsv, int startRow, int endRow, int startCol, int endCol)
    {
        string[] lines = tsv.Split('\n');
        if (lines.Length < 2) return "[]";

        string[] allHeaders = lines[0].Trim().Split('\t');

        int lastCol = (endCol == -1) ? allHeaders.Length - 1 : Mathf.Min(endCol, allHeaders.Length - 1);
        string[] headers = allHeaders[startCol..(lastCol + 1)];

        JArray jsonArray = new JArray();

        int lastRow = (endRow == -1) ? lines.Length - 1 : Mathf.Min(endRow, lines.Length - 1);

        for (int i = startRow; i <= lastRow; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = lines[i].Trim().Split('\t');
            JObject jsonObject = new JObject();

            for (int j = startCol; j <= lastCol && j < values.Length; j++)
            {
                // headers[j - startCol]을 사용해 범위에 맞는 header를 참조
                jsonObject[headers[j - startCol]] = values[j].Trim();
            }

            jsonArray.Add(jsonObject);
        }

        return jsonArray.ToString();
    }


    private void ApplyDataToSO(JArray jsonData, bool renameFiles)
    {
        ClearAllDataSO();
        abilityDataSO.Clear();

        for (int i = 0; i < jsonData.Count; i++)
        {
            JObject row = (JObject)jsonData[i];

            int index = int.TryParse(row["Index"]?.ToString(), out int parsedindex) ? parsedindex : default;
            string dataname = row["Name"]?.ToString() ?? "";
            int HP = int.TryParse(row["HP"]?.ToString(), out int parsedHP) ? parsedHP : default;
            int attackPower = int.TryParse(row["AttackPower"]?.ToString(), out int parsedAttackPower) ? parsedAttackPower : default;
            int attackSpeed = int.TryParse(row["AttackSpeed"]?.ToString(), out int parsedAttackSpeed) ? parsedAttackSpeed : default;
            string description = row["Descripsion"]?.ToString() ?? "";


            DataSO data = ScriptableObject.CreateInstance<DataSO>();

            // 기존 SO 개수가 부족하면 새로 생성
            if (i < abilityDataSO.Count)
            {
                data = abilityDataSO[i];
            }
            else
            {
                data = CreateNewDataSO(dataname); // 새로운 SO 생성
                abilityDataSO.Add(data);
            }

            if (renameFiles)
            {
                RenameScriptableObjectFile(data, dataname);
            }

            data.SetData(index, dataname, HP, attackPower, attackSpeed, description);
            EditorUtility.SetDirty(data);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void RenameScriptableObjectFile(DataSO so, string newFileName)
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(so);
        string newPath = Path.GetDirectoryName(path) + "/" + newFileName + ".asset";

        if (path != newPath)
        {
            AssetDatabase.RenameAsset(path, newFileName);

            // 즉시 저장하여 반영
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }

    /// <summary>
    /// 지정된 폴더 내의 모든 ScriptableObject(AbilityDataSO) 파일을 삭제하는 함수.
    /// </summary>
    private void ClearAllDataSO()
    {
        string folderPath = "Assets/Team/KDS/ScriptableObject/Data";

        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning("SO 폴더가 존재하지 않음");
            return;
        }

        string[] files = Directory.GetFiles(folderPath, "*.asset");

        foreach (string file in files)
        {
            AssetDatabase.DeleteAsset(file);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private DataSO CreateNewDataSO(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            fileName = "DefaultData";
        }

        DataSO newSO = ScriptableObject.CreateInstance<DataSO>();

#if UNITY_EDITOR
        string folderPath = "Assets/Team/KDS/ScriptableObject/Data";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string assetPath = $"{folderPath}/{fileName}.asset";
        AssetDatabase.CreateAsset(newSO, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

#endif
        return newSO;
    }

    /////////////////////////////////////////////////////////기본템플릿///////////////////////////////////////////
    ///

    /////////////////////////////////결합정보테이블//////////////////////////////////////////////////////////////
    const string URL_CombinationData = "https://docs.google.com/spreadsheets/d/1WV9YaIFWGZ6o0EonAEMNsEbMHrbqGUoJgYVA3oZbQFQ/export?format=tsv&gid=695451767";

    public void StartCombinationDataDownload(bool renameFiles)
    {
        StartCoroutine(DownloadCombinationData(renameFiles));
    }

    private IEnumerator DownloadCombinationData(bool renameFiles)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL_CombinationData);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            string tsvText = www.downloadHandler.text;
            string json = ConvertTSVToJson(tsvText, startRow: 2, endRow: 98, startCol: 0, endCol: 3);
            JArray jsonData = JArray.Parse(json); // JSON 문자열을 JArray로 변환
            ApplyCombinationDataToSO(jsonData, renameFiles);
        }
        else
        {
            Debug.LogError("데이터 가져오기 실패: " + www.error);
        }
    }

    public void ApplyCombinationDataToSO(JArray jsonData, bool renameFiles)
    {
        towerCombinationData.combinationRules.Clear();
        for (int i = 0; i < jsonData.Count; i++)
        {

            JObject row = (JObject)jsonData[i];
            foreach (var prop in row.Properties())
            {
                Debug.Log($"Key: '{prop.Name}', Value: '{prop.Value}'");
            }
            int result = int.TryParse(row["fusionIndex"]?.ToString(), out int parsedResult) ? parsedResult : default;
            int ingredient1 = int.TryParse(row["tower_material1"]?.ToString(), out int parsedIngredient1) ? parsedIngredient1 : default;
            int ingredient2 = int.TryParse(row["tower_material2"]?.ToString(), out int parsedIngredient2) ? parsedIngredient2 : default;
            Debug.Log($"result: {result}, ingredient1: {ingredient1}, ingredient2: {ingredient2}");
            towerCombinationData.SetData(result, ingredient1, ingredient2);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
