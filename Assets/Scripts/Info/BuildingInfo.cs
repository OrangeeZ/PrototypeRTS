using System;
using System.Collections;
using System.Collections.Generic;
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

    public void Configure(Values values)
    {
    }
}
