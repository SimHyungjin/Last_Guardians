using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;


[CustomEditor(typeof(DataDownLoadManager))]
public class SheetDownButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DataDownLoadManager fnc = (DataDownLoadManager)target;
        if (GUILayout.Button("Download SheetData"))
        {
            fnc.StartDownload(true);
        }
    }
}


public class DataDownLoadManager : MonoBehaviour
{
    [SerializeField] private List<DataSO> abilityDataSO = new List<DataSO>();
    const string URL_DataSheet= "https://docs.google.com/spreadsheets/d/11uh3qkFXuMsowtu7qrmO66nYMx46C2P9UHh3c1eHVu4/export?format=tsv&range=A2:F6";

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

        // CreatePrefabs();
        ApplyDataSO();
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
}
