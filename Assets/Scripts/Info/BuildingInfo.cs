using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Actors;
using csv;
using UnityEngine;

public class BuildingInfo : ScriptableObject, ICsvConfigurable
{
    [RemoteProperty("Name")]
    public string Name;

    [RemoteProperty("HP")]
    public int Hp;

    [RemoteProperty("InputResource")]
    public string InputResource;

    [RemoteProperty("InputResourceQuantity")]
    public int InputResourceQuantity;

    [RemoteProperty("ProductionDuration")]
    public int ProductionDuration;

    [RemoteProperty("OutputResource")]
    public string OutputResource;

    [RemoteProperty("OutputResourceQuantity")]
    public int OutputResourceQuantity;

    public ActorView Prefab;

    public void Configure(Values values)
    {
        Prefab = values.GetPrefabWithComponent<ActorView>("Prefab", false);
    }
}
