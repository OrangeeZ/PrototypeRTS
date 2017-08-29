using System.Collections.Generic;
using csv;
using UnityEngine;

public enum ResourceType
{
    None,
    Food,
}

public class ResourceInfo : ScriptableObject, ICsvConfigurable
{

    [RemoteProperty]
    public string Id;
    
    [RemoteProperty]
    public string Name;

    [RemoteProperty]
    public int MaxCountPerStockpileTile;

    public ResourceType ResourceType;

    public void Configure(Values values)
    {
        values.GetEnum<ResourceType>("ResourceType", out ResourceType, ResourceType.None);
    }
}
