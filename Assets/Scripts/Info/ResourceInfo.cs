using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Actors;
using csv;
using UnityEngine;

public class ResourceInfo : ScriptableObject, ICsvConfigurable
{
    [RemoteProperty("Name")]
    public string Name;

    [RemoteProperty("MaxCountPerStockpileTile")]
    public int MaxCountPerStockpileTile;

    public void Configure(Values values)
    {
    }
}
