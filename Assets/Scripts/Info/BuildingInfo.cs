using System.Collections.Generic;
using Actors;
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

    [RemoteProperty]
    public List<ProductionCyclesInfo> ProductionCycles;

    public void Configure(Values values)
    {
    }
}