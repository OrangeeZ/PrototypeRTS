using System.Collections.Generic;
using Assets.Scripts.Actors;
using csv;
using UnityEngine;

public class BuildingInfo : ScriptableObject, ICsvConfigurable
{
    [RemoteProperty]
    public string Name;

    [RemoteProperty]
    public string Id;

    [RemoteProperty]
    public int Hp;

    [RemoteProperty]
    public ActorView Prefab;

    [RemoteProperty]
    public EntityDisplayPanel DisplayPanelPrefab;
    
    public List<ProductionCyclesInfo> ProductionCycles;

    public void Configure(Values values)
    {
        var items = values.GetScriptableObjects<ProductionCyclesInfo>("productioncycles");
        if(items!=null)
            ProductionCycles = new List<ProductionCyclesInfo>(items);
    }
}
