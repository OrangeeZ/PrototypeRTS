using csv;
using UnityEngine;

public class ProductionCyclesInfo : ScriptableObject, ICsvConfigurable
{
    [RemoteProperty]
    public string Id;

    [RemoteProperty]
    public string Name;

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

    public void Configure(Values values)
    {
    }

    public void OnPostLoad()
    {
        
    }
}