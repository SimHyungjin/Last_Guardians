using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public static class AnalyticsLogger
{
    public static bool IsServiceOn = false;

    public static bool ShouldSkipAnalytics()
    {
        //작동 중지
        if (!IsServiceOn || UnityServices.State != ServicesInitializationState.Initialized)
            return true;
        //작동
        return false;
    }

    public static void LogTowerSelect(List<TowerData> selectedCards)
    {
        if (ShouldSkipAnalytics()) return;

        for (int i = 0; i < selectedCards.Count; i++)
        {
            var customEvent = new CustomEvent("Tower_Select");
            customEvent.Add("Tower_Name", selectedCards[i].TowerName);
            if (i < 2) customEvent.Add("Tower_Section", 1);
            else if (i < 3) customEvent.Add("Tower_Section", 2);
            else customEvent.Add("Tower_Section", 3);

            AnalyticsService.Instance.RecordEvent(customEvent);
        }
        Debug.Log("LogTowerSelect Complete");
    }

    public static void LogTowerUpgrade(TowerUpgradeData data)
    {
        if (ShouldSkipAnalytics()) return;

        for (int i = 0; i < data.currentLevel.Count; i++)
        {
            var customEvent = new CustomEvent("Tower_Upgrade");
            customEvent.Add("Towerupgrade_Name", ((TowerUpgradeType)i).ToString());
            customEvent.Add("Towerupgrade_Resource", data.totalMasteryPoint);
            customEvent.Add("Towerupgrade_Level", data.currentLevel[i]);
            AnalyticsService.Instance.RecordEvent(customEvent);
        }
        Debug.Log("LogTowerUpgrade Complete");
    }

    public static void LogWaveEnd(bool isGameOver, int wave)
    {
        if (ShouldSkipAnalytics()) return;

        var customEvent = new CustomEvent("Wave_End");
        customEvent.Add("Gameover_Type", isGameOver ? "failed" : "exit");
        customEvent.Add("Gameover_Wave", wave);

        AnalyticsService.Instance.RecordEvent(customEvent);
        Debug.Log("LogWaveEnd Complete");
    }

    public static void LogUserEquip(int index, int wave)
    {
        if (ShouldSkipAnalytics()) return;
        var customEvent = new CustomEvent("User_Equip");
        customEvent.Add("Item_Index", index);
        customEvent.Add("User_Wave", wave);
        AnalyticsService.Instance.RecordEvent(customEvent);
        Debug.Log("LogUserEquip Complete");
    }
}
