using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(ScreenshotTool))]
public class ScreenshotToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ScreenshotTool tool = (ScreenshotTool)target;

        if (GUILayout.Button("캡처 저장"))
        {
            CaptureScreenshot(tool);
        }
    }

    private void CaptureScreenshot(ScreenshotTool tool)
    {
        if (tool.captureCamera == null)
        {
            Debug.LogError("캡처할 카메라를 지정하세요.");
            return;
        }

        RenderTexture rt = new RenderTexture(tool.width, tool.height, 24);
        tool.captureCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(tool.width, tool.height, TextureFormat.RGB24, false);

        tool.captureCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, tool.width, tool.height), 0, 0);
        screenShot.Apply();

        tool.captureCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);

        string folderPath = "Screenshots";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string filePath = $"{folderPath}/Screenshot_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        File.WriteAllBytes(filePath, screenShot.EncodeToPNG());

        Debug.Log($"스크린샷 저장됨: {filePath}");
        AssetDatabase.Refresh(); // 프로젝트 창에서 바로 보이게 함
    }
}
