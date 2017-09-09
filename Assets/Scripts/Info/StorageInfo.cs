using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csv;
using UnityEngine;

public class ResourceFilter
{
    public ResourceInfo Resource;
    public int MaxAmount;
}

public class StorageInfo : ScriptableObject, ICsvConfigurable
{
    [RemoteProperty]
    public string BuildingId;

    [RemoteProperty]
    public int SlotsCount;

    public ResourceType[] AllowedTypes;

    public ResourceFilter[] AllowedResources;

    public void Configure(Values values)
    {
        ResourceType type;
        values.GetEnum("allowedtypes", out type);

        AllowedTypes = new [] {type};
    }

    public void PostLoad()
    {
        
    }

    public void Init(IEnumerable<ResourceInfo> resources)
    {
        var resourceInfos = resources.Where(info => AllowedTypes.Contains(info.ResourceType)).ToArray();
        AllowedResources = new ResourceFilter[resourceInfos.Length];
        for (var i = 0; i < AllowedResources.Length; ++i)
        {
            AllowedResources[i] = new ResourceFilter {MaxAmount = int.MaxValue, Resource = resourceInfos[i]};
        }
    }
}
