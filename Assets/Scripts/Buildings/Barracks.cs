using System.Collections.Generic;
using Assets.Scripts.Actors;

public class Barracks : Building
{
    public List<UnitInfo> AvailableUnits { get; protected set; }

    private readonly TestUnitFactory _unitFactory;

    public Barracks(List<UnitInfo> barraksUnits, BaseWorld world, TestUnitFactory unitFactory) : base(world)
    {
        AvailableUnits = barraksUnits;

        _unitFactory = unitFactory;
    }

    public bool CanHireUnit(UnitInfo unitInfo)
    {
        var stockpile = World.Stockpile;
        var hasArmor = unitInfo.RequiredArmor == null || stockpile.HasResource(unitInfo.RequiredArmor);
        var hasWeapon = unitInfo.RequiredWeapon == null || stockpile.HasResource(unitInfo.RequiredWeapon);
        
        return hasArmor && hasWeapon;
    }

    public void HireUnit(UnitInfo unitInfo)
    {
        _unitFactory.CreateUnit(unitInfo);
    }
}