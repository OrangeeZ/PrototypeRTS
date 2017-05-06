using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Workplace;
using UnityEngine;

public class Stockpile : MonoBehaviour
{
    private Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>();

    public bool HasResource(ResourceType resouceType)
    {
        return _resources.ContainsKey(resouceType) && _resources[resouceType] > 0;
    }

    public void AddResource(ResourceType resourceType, int amount)
    {
		if (!_resources.ContainsKey(resourceType))
		{
			_resources.Add(resourceType, 0);
		}

        _resources[resourceType] += amount;

		Debug.LogFormat("Added {0}, amount {1}", resourceType, amount);
    }

    public void RemoveResource(ResourceType resourceType, int amount)
    {
        _resources[resourceType] -= amount;
    }
}
