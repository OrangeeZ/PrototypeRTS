using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StockpileBlock : Building
{
    #region private properties

    private readonly StorageInfo _storageInfo;
    private readonly Dictionary<ResourceInfo, int> _resources = new Dictionary<ResourceInfo, int>();
    private readonly Dictionary<string, ResourceInfo> _resourcesIds = new Dictionary<string, ResourceInfo>();

    #endregion

    #region constructor

    public StockpileBlock(BaseWorld world, StorageInfo storageInfo)
        : base(world)
    {
        _storageInfo = storageInfo;
        foreach (var resource in storageInfo.AllowedResources.Select(filter => filter.Resource))
        {
            _resourcesIds[resource.Id] = resource;
            _resources[resource] = 0;
        }
    }

    #endregion

    #region public properties

    public string[] ResourceIds => _resources.Keys.Select(x => x.Id).ToArray();

    /// <summary>
    /// amount of target resource
    /// </summary>
    public int this[string resource] => GetResourceAmount(resource);

    #endregion

    public bool CanStore(ResourceInfo resource, int amount)
    {
        return _storageInfo.AllowedResources.Any(filter => filter.Resource.Id == resource.Id);
    }

    public bool HasResource(string resource)
    {
        return GetResourceAmount(resource) > 0;
    }

    public bool ChangeResource(string resource, int amount)
    {
        var resourceItem = GetResource(resource);
        if (resourceItem == null) return false;
        var resourceAmount = GetResourceAmount(resource);
        var result = resourceAmount + amount;
        _resources[resourceItem] = result < 0 ? 0 : result;
        Debug.Log($"Change resource {resource}, amount {amount}");
        return true;
    }

    public int GetResourceAmount(string id)
    {
        if(!_resourcesIds.ContainsKey(id))return 0;
        var resource =_resourcesIds[id];
        if (!_resources.ContainsKey(resource)) return 0;
        return _resources[resource];
    }

    #region private methods

    private ResourceInfo GetResource(string id)
    {
        if (!_resourcesIds.ContainsKey(id)) return null;
        var resource = _resourcesIds[id];
        return resource;
    }

    #endregion

}