using Assets.Scripts.Actors;

public class Building : Entity {

    public Building(BaseWorld world) : base(world)
    {

    }
    
    public BuildingInfo Info { get; private set; }

    #region public methods
    
    public override void Update(float deltaTime)
    {
        
    }
    
    public virtual void SetInfo(BuildingInfo info)
    {
        Info = info;
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
