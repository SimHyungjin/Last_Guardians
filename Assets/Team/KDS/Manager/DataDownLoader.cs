using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;


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
    }
}


public class DataDownLoader : MonoBehaviour
{
    [SerializeField] private List<DataSO> abilityDataSO = new List<DataSO>();
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

            JArray jsonData = JArray.Parse(json); // JSON ���ڿ��� JArray�� ��ȯ
            ApplyDataToSO(jsonData, renameFiles);
        }
        else
        {
            Debug.LogError("������ �������� ����: " + www.error);
        }
    }

    string ConvertTSVToJson(string tsv)
    {
        string[] lines = tsv.Split('\n'); // �� ������ �и�
        if (lines.Length < 2) return "[]"; // �����Ͱ� ������ �� JSON �迭 ��ȯ

        string[] headers = lines[0].Split('\t'); // ù ��° ���� ����� ���
        JArray jsonArray = new JArray();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split('\t'); // ������ �� �и�
            JObject jsonObject = new JObject();

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                string cleanValue = values[j].Trim();
                jsonObject[headers[j].Trim()] = cleanValue; // `+` ���ڵ� �� ��
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


            DataSO data= ScriptableObject.CreateInstance<DataSO>();

            // ���� SO ������ �����ϸ� ���� ����
            if (i < abilityDataSO.Count)
            {
                data = abilityDataSO[i];
            }
            else
            {
                data = CreateNewDataSO(dataname); // ���ο� SO ����
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

            // ��� �����Ͽ� �ݿ�
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }

    /// <summary>
    /// ������ ���� ���� ��� ScriptableObject(AbilityDataSO) ������ �����ϴ� �Լ�.
    /// </summary>
    private void ClearAllDataSO()
    {
        string folderPath = "Assets/Team/KDS/ScriptableObject/Data";

        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning("SO ������ �������� ����");
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

}
