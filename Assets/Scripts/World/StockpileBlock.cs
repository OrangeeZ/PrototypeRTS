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
        : base(world)
    {
    }

    public string[] Resources
    {
        get { return _resources.Keys.ToArray(); }
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

    public void ChangeResource(string resource, int amount)
    {
        if (!_resources.ContainsKey(resource))
        {
            _resources[resource] = 0;
        }
        
        var result = _resources[resource] + amount;
        _resources[resource] = result < 0 ? 0 : result;
        
        Debug.Log($"Change resource {resource}, amount {amount}");
    }
}