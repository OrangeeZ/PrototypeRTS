using System.Collections.Generic;
using Assets.Scripts.Actors;

public class Barracks : Building
{
    private readonly TestUnitFactory _unitFactory;

    #region constructor

    public Barracks(List<UnitInfo> barraksUnits,BaseWorld world,
        TestUnitFactory unitFactory) : 
        base(world)
    {
        AvailableUnits = barraksUnits;
        _unitFactory = unitFactory;
    }

    #endregion

    public List<UnitInfo> AvailableUnits { get; protected set; }

    #region public methods

    public void HireArmyUnit(UnitInfo unitInfo)
    {
        _unitFactory.CreateUnit(unitInfo);
    }

    #endregion
}
