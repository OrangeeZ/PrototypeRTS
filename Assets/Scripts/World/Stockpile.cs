using System.Collections.Generic;
using UnityEngine;

public class Stockpile : MonoBehaviour
{
    private Dictionary<string, int> _resources = new Dictionary<string, int>();

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
        if(!_resources.ContainsKey(resource))
            return;
        _resources[resource] -= amount;
    }
}
