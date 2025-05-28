using System;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class ServiceInitializer : MonoBehaviour
{
    private async void Awake()
    {
        if (!AnalyticsLogger.IsServiceOn) return;
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();


    }
}
