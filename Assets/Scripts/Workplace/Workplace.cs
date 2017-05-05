using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkplaceState
{
    WaitingForNewWorker,
    FetchingResources,
    Producing,
    DeliveringResources
}

public enum ResourceType
{
    Bread,
    Wood,
}

public abstract class Workplace
{
    protected IntelligentAgent Worker;

    private WorkplaceState _currentWorkplaceState = WorkplaceState.WaitingForNewWorker;
    private ResourceType _resourceType;

    public void SetWorker(IntelligentAgent actor)
    {
        Worker = actor;
    }

    public void SetResourceType(ResourceType resourceType)
    {
        _resourceType = resourceType;
    }

    public void Update(float deltaTime)
    {
        if (Worker == null || !Worker.IsOrderCompleted)
        {
            return;
        }

        Worker.SetOrder(new DeliverResourcesOrder(_resourceType, 1));
    }
}
