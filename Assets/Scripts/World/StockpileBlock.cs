using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;

public class StorageSlot
{
    public ResourceInfo Resource;
    public int Amount;
    public int Capacity => Resource == null ? 0 : Resource.MaxCountPerStockpileTile;

    public void Store(ResourceInfo resource, int amount)
    {
        if (IsEmpty)
        {
            Resource = resource;
            Amount = amount > 0 ? amount : 0;
            return;
        }

        if(Resource.Id != resource.Id)
            throw new InvalidOperationException($"Can't store {resource.Id} slot already occupied by {Resource.Id}");

        Amount += amount;

        if (Amount < 0)
            Amount = 0;
    }

    public bool IsEmpty => Amount == 0 || Resource == null;
    public bool IsFull => Amount >= Capacity;
    public int FreeSpace => Capacity - Amount;

    public static StorageSlot Empty = new StorageSlot();
}

public class StockpileBlock : Building
{
    #region private properties

    private readonly StorageInfo _storageInfo;
    private readonly StorageSlot[] _storageSlots;
    private readonly Dictionary<string, ResourceInfo> _resourcesIds = new Dictionary<string, ResourceInfo>();

    #endregion

    #region constructor

    public StockpileBlock(BaseWorld world, StorageInfo storageInfo)
        : base(world)
    {
        _storageSlots = Enumerable.Range(0, storageInfo.SlotsCount).Select(i => new StorageSlot()).ToArray();
        _storageInfo = storageInfo;
        foreach (var resource in storageInfo.AllowedResources.Select(filter => filter.Resource))
        {
            _resourcesIds[resource.Id] = resource;
        }
    }

    #endregion

    #region public properties

    public StorageInfo StorageInfo => _storageInfo;
    public string[] ResourceIds => _resourcesIds.Values.Select(x => x.Id).ToArray();

    /// <summary>
    /// amount of target resource
    /// </summary>
    public int this[string resource] => GetResourceAmount(resource);

    #endregion

    public StorageSlot GetSlot(int index)
    {
        return _storageSlots[index];
    }

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
        var result = StoreResource(resourceItem, amount);
        if (!result)
        {
            Debug.LogError($"Can't store resource {resource} ({amount})");
            return false;
        }
        Debug.Log($"Change resource {resource}, amount {amount}");
        return true;
    }

    public int GetResourceAmount(string id)
    {
        if(!_resourcesIds.ContainsKey(id)) return 0;
        return _storageSlots
            .Where(slot => !slot.IsEmpty && slot.Resource.Id == id)
            .DefaultIfEmpty(StorageSlot.Empty)
            .Sum(slot => slot.Amount);
    }

    #region private methods

    private StorageSlot GetFreeSlot()
    {
        return _storageSlots.FirstOrDefault(slot => slot.IsEmpty);
    }

    private bool StoreResource(ResourceInfo resource, int amount)
    {
        if(amount > 0)
            return PutResource(resource, amount);

        if (amount < 0)
            return GetResource(resource, -amount);

        return true;
    }

    private bool GetResource(ResourceInfo resource, int amount)
    {
        var slotsWithResource = _storageSlots.Where(slot => !slot.IsEmpty && slot.Resource.Id == resource.Id).ToArray();
        if (slotsWithResource.Sum(slot => slot.Amount) < amount)
            return false;

        foreach (var storageSlot in slotsWithResource)
        {
            var amountToGet = Math.Min(amount, storageSlot.Amount);
            storageSlot.Store(resource, -amountToGet);
            amount -= amountToGet;
            if(amount <= 0)
                break;
        }

        return true;
    }

    private bool PutResource(ResourceInfo resource, int amount)
    {
        var slotsWithResource = _storageSlots.Where(slot => !slot.IsEmpty && slot.Resource.Id == resource.Id).ToArray();
        var freeSpace = slotsWithResource.Sum(slot => slot.FreeSpace);
        if (freeSpace < amount)
        {
            freeSpace += _storageSlots.Count(slot => slot.IsEmpty) * resource.MaxCountPerStockpileTile;
            if (freeSpace < amount)
            {
                Debug.LogWarning($"Not enough space for {resource.Id} ({amount}). Only {freeSpace} free space left");
                return false;
            }
        }

        foreach (var storageSlot in slotsWithResource)
        {
            var amountToPut = Math.Min(storageSlot.FreeSpace, amount);
            storageSlot.Store(resource, amountToPut);
            amount -= amountToPut;
            if(amount <= 0)
                break;
        }

        while (amount > 0)
        {
            var slot = GetFreeSlot();
            var amountToPut = Math.Min(slot.FreeSpace, amount);
            slot.Store(resource, amountToPut);
            amount -= amountToPut;
        }

        return true;
    }

    private ResourceInfo GetResource(string id)
    {
        if (!_resourcesIds.ContainsKey(id)) return null;
        var resource = _resourcesIds[id];
        return resource;
    }

    #endregion

}