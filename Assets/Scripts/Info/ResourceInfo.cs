using System.Collections.Generic;
using csv;
using UnityEngine;

public enum ResourceType
{
    None,
    Food,
    Military
}

public class ResourceInfo : ScriptableObject, ICsvConfigurable
{
    [RemoteProperty]
    public string Id;
    
    [RemoteProperty]
    public string Name;

    [RemoteProperty]
    public int MaxCountPerStockpileTile;

    [RemoteProperty]
    public ResourceType ResourceType;

    [RemoteProperty]
    public UnitInfo AssociatedUnitInfo;
    
    public void Configure(Values values)
    {
    }

    public void OnPostLoad()
    {
        
    }
}
