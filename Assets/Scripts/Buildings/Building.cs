using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actors;

public class Building : Entity {

    #region constructor

    public Building(BaseWorld world) : base(world)
    {
        Available = new List<ProductionCyclesInfo>();
    }

    #endregion

    #region public properties

    public List<ProductionCyclesInfo> Available { get; protected set; }

    public ProductionCyclesInfo ActiveProductionCycle { get; protected set; }

    public BuildingInfo Info { get; private set; }

    #endregion

    #region public methods

    public override void Update(float deltaTime)
    {
        
    }
    
    public virtual void SetInfo(BuildingInfo info)
    {
        Info = info;
        Available.AddRange(Info.ProductionCycles);
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
