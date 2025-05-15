using System;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class ServiceInitializer : MonoBehaviour
{
    private async void Awake()
    {
<<<<<<< Updated upstream
        if (AnalyticsLogger.ShouldSendAnalytics()) return;
=======
        if (!AnalyticsLogger.IsServiceOn) return;
>>>>>>> Stashed changes
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("Unity Analytics 초기화 완료");
        }
        catch (Exception e)
        {
            Debug.LogError("Unity Services 초기화 실패: " + e.Message);
        }
    }
}
