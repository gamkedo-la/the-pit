using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class GameBuildInfoProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    
    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("Preparing for build...");
        
        var guids = AssetDatabase.FindAssets($"t:{typeof(GameBuildInfo)}");
        if (guids.Length != 1)
        {
            Debug.LogError($"Expected 1 of GameBuildInfo, but found {guids.Length}.");
            return;
        }
        
        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        var buildInfo = AssetDatabase.LoadAssetAtPath<GameBuildInfo>(path);
        
        AssetDatabase.StartAssetEditing();
        try
        {
            buildInfo.lastBuildTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
            buildInfo.buildNumber += 1;
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
        }
        
        EditorUtility.SetDirty(buildInfo);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}