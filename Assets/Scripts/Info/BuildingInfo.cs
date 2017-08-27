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
    public ResourceInfo InputResource;

    [RemoteProperty]
    public int InputResourceQuantity;

    [RemoteProperty]
    public int ProductionDuration;

    [RemoteProperty]
    public ResourceInfo OutputResource;

    [RemoteProperty]
    public int OutputResourceQuantity;
    
    [RemoteProperty]
    public ActorView Prefab;

    [RemoteProperty]
    public EntityDisplayPanel DisplayPanelPrefab;

    public void Configure(Values values)
    {
    }
}
