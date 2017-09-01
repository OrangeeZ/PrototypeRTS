<<<<<<< f8148aa54878b436da711513ce755c353fa6977b
﻿using System.Collections.Generic;
using System.Linq;
using Actors;
using Assets.Scripts.Actors;
=======
﻿using Actors;
>>>>>>> 51109a5ae2f0af8e4c1aa3bacf25fb4abc855286

public class Building : Entity {

    #region constructor

    public Building(BaseWorld world) : base(world)
    {
        AvailableProduction = new List<ProductionCyclesInfo>();
    }

    #endregion

    #region public properties

    public List<ProductionCyclesInfo> AvailableProduction { get; protected set; }

    public ProductionCyclesInfo ActiveProductionCycle { get; protected set; }

    public BuildingInfo Info { get; private set; }

    #endregion

    #region public methods

    public bool ActivateProduction(ProductionCyclesInfo cyclesInfo)
    {
        if(!AvailableProduction.Contains(cyclesInfo))
            return false;
        ActiveProductionCycle = cyclesInfo;
        return true;
    }

    public override void Update(float deltaTime)
    {
        
    }
    
    public virtual void SetInfo(BuildingInfo info)
    {
        Info = info;
        AvailableProduction.AddRange(Info.ProductionCycles);
        ActiveProductionCycle = Info.ProductionCycles.FirstOrDefault();
    }

    public override EntityDisplayPanel GetDisplayPanelPrefab()
    {
        return Info.DisplayPanelPrefab;
    }

    public override void Kill()
    {
        SetState(false);
        base.Kill();
    }

    #endregion


    
}
