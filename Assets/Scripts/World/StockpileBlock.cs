using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Actors;
using UnityEngine;

public class StockpileBlock : Building
{
    private Dictionary<string, int> _resources = new Dictionary<string, int>();

    public StockpileBlock(BaseWorld world)
        :base(world)
    {
    }

    /// <summary>
    /// amount of target resource
    /// </summary>
    public int this[string resource]
    {
        get
        {
            if (_resources.ContainsKey(resource))
                return _resources[resource];
            return -1;
        }
    }

    public bool HasResource(string resource)
    {
        return _resources.ContainsKey(resource) && _resources[resource] > 0;
    }

    public void AddResource(string resource, int amount)
    {
        if (!_resources.ContainsKey(resource))
        {
            _resources.Add(resource, 0);
        }

        _resources[resource] += amount;

        Debug.LogFormat("Added {0}, amount {1}", resource, amount);
    }

    public void RemoveResource(string resource, int amount)
    {
        if (!_resources.ContainsKey(resource))
            return;
        _resources[resource] -= amount;
    }
}
