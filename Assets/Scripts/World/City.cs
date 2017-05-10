using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Actors;
using Assets.Scripts.Workplace;
using UnityEngine;

public interface ICity
{
    int FreeCitizensCount { get; }
    void RegisterFreeCitizen(Actor actor);
    void GetClosestStockpileWithResource(ResourceType resourceType);
    Stockpile GetClosestStockpile(Vector3 position);
    Actor GetFreeCitizen();
    Transform GetFireplace();
}
