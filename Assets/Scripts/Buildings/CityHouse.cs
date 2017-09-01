public class CityHouse : Building {

    public CityHouse(BaseWorld world) : base(world)
    {
        
    }

    #region public methods

    public override void SetInfo(BuildingInfo info)
    {
        base.SetInfo(info);
        World.PopulationLimit += ActiveProductionCycle.OutputResourceQuantity;
    }

    public override void Kill()
    {
        base.Kill();
        World.PopulationLimit -= ActiveProductionCycle.OutputResourceQuantity;
    }

    #endregion
}
