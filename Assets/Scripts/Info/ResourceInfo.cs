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

    public ResourceType ResourceType;

    public void Configure(Values values)
    {
        values.GetEnum<ResourceType>("resourcetype", out ResourceType, ResourceType.None);
    }
}
