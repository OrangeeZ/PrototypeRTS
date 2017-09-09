using System.Linq;
using csv;
using UnityEngine;

public class WorldInfo : ScriptableObject, ICsvConfigurable
{
    [SerializeField]
    public ResourceInfo[] ResourceInfos;

    [SerializeField]
    public StorageInfo[] StorageInfos;

    [SerializeField]
    public UnitInfo[] UnitInfos;

    [SerializeField]
    public BuildingInfo[] BuildingInfos;

    [RemoteProperty]
    public string Name;

    [RemoteProperty]
    public int MinPopulation;

    [RemoteProperty]
    public int MaxPopulation;

    [RemoteProperty]
    public int Tax;

    [RemoteProperty]
    public int StartGold;

    [RemoteProperty]
    public int MaxPopularity = 100;

    [RemoteProperty]
    public int MinPopularity = 0;

    public void Configure(Values values)
    {
        
    }

    public void OnPostLoad()
    {
        HookData();
    }

    [ContextMenu("Hook data")]
    private void HookData()
    {
        UnitInfos = UnityEditor.AssetDatabase
            .FindAssets("t:unitinfo")
            .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
            .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<UnitInfo>)
            .ToArray();

        BuildingInfos = UnityEditor.AssetDatabase
            .FindAssets("t:buildinginfo")
            .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
            .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<BuildingInfo>)
            .ToArray();

        ResourceInfos = UnityEditor.AssetDatabase
            .FindAssets("t:resourceinfo")
            .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
            .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<ResourceInfo>)
            .ToArray();

        StorageInfos = UnityEditor.AssetDatabase
            .FindAssets("t:storageinfo")
            .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
            .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<StorageInfo>)
            .ToArray();
    }
}
