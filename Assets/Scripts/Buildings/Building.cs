using Assets.Scripts.Actors;

public class Building : Entity {

    #region constructor

    public Building(BaseWorld world) : base(world)
    {
        
    }

    #endregion

    #region public properties

    public ProductionCyclesInfo ProductionCycle { get; protected set; }

    public BuildingInfo Info { get; private set; }

    #endregion

    #region public methods

    public override void Update(float deltaTime)
    {
        
    }
    
    public virtual void SetInfo(BuildingInfo info)
    {
        Info = info;
        ProductionCycle = Info.ProductionCycles;
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
