using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Actors;
using csv;
using UnityEngine;

public class ResourceInfo : ScriptableObject, ICsvConfigurable
{
    [RemoteProperty]
    public string Id;
    
    [RemoteProperty]
    public string Name;

    [RemoteProperty]
    public int MaxCountPerStockpileTile;

    public void Configure(Values values)
    {
    }
}
