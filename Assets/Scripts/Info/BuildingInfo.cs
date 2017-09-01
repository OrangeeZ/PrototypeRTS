<<<<<<< f8148aa54878b436da711513ce755c353fa6977b
﻿using System.Collections.Generic;
using Assets.Scripts.Actors;
=======
﻿using Actors;
>>>>>>> 51109a5ae2f0af8e4c1aa3bacf25fb4abc855286
using csv;
using UnityEngine;

public class BuildingInfo : ScriptableObject, ICsvConfigurable
{
    [RemoteProperty]
    public string Name;

    [RemoteProperty]
    public string Id;

    [RemoteProperty]
    public int Hp;

    [RemoteProperty]
    public ActorView Prefab;

    [RemoteProperty]
    public EntityDisplayPanel DisplayPanelPrefab;

    [RemoteProperty]
    public List<ProductionCyclesInfo> ProductionCycles;

    public void Configure(Values values)
    {
    }
}
