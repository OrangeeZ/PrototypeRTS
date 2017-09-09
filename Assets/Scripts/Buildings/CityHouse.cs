public class CityHouse : Building {

    public CityHouse(BaseWorld world) : base(world)
    {
        
    }

    #region public methods

    public override void SetInfo(BuildingInfo info)
    {
        base.SetInfo(info);
        World.MaxPopulation += ActiveProductionCycle.OutputResourceQuantity;
    }

    public override void Kill()
    {
        base.Kill();
        World.MaxPopulation -= ActiveProductionCycle.OutputResourceQuantity;
    }

    #endregion
}
