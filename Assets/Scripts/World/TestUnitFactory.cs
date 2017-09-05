using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Actors;
using Assets.Scripts.World;
using Behaviour;
using Buildings;
using UnityEngine;

public class TestUnitFactory : MonoBehaviour
{
    public byte FactionId;

    public UnitInfo[] UnitInfos => _worldInfo.UnitInfos;

    public BuildingInfo[] BuildingInfos => _worldInfo.BuildingInfos;

    private BaseWorld _world;
    private Dictionary<string, Type> _behaviourMap;
    private List<UnitInfo> _armyUnitsInfos = new List<UnitInfo>();
    private WorldInfo _worldInfo;


    public void SetWorld(BaseWorld world, WorldInfo worldInfo)
    {
        _world = world;
        _worldInfo = worldInfo;
        BuildTypeMap();
    }

    public Entity CreateUnit(UnitInfo unitInfo)
    {
        //no free citizen or population limit reached
        if (_world.Population >= _world.PopulationLimit)
        {
            return null;
        }

        var unit = new Actor(_world);
        _world.Entities.Add(unit);

        var randomPosition = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(5, 10f);
        randomPosition.y = 0;
        unit.SetPosition(_world.GetFireplace() + randomPosition);

        return CreateUnit(unitInfo, unit);
    }

    public Entity CreateUnit(UnitInfo unitInfo, Actor existingUnit)
    {
        var unit = existingUnit;
        var unitView = Instantiate(unitInfo.Prefab);

        unit.SetView(unitView);
        unit.SetBehaviour(CreateBehaviour(unitInfo.BehaviourId));

        unit.SetInfo(unitInfo);
        unit.SetHealth(unitInfo.Hp);
        unit.SetFactionId(FactionId);

        return unit;
    }

    public Entity CreateBuilding(BuildingInfo buildingInfo)
    {
        var building = CreateBuildingEntity(buildingInfo);
        building.SetView(Instantiate(buildingInfo.Prefab));
        building.SetInfo(buildingInfo);
        var randomPosition = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(5, 10f);
        randomPosition.y = 0;
        building.SetPosition(_world.GetFireplace() + randomPosition);
        building.SetHealth(10);
        _world.Entities.Add(building);
        return building;
    }

    #region private methods

    private Building CreateBuildingEntity(BuildingInfo buildingInfo)
    {
        switch (buildingInfo.Id)
        {
            case "Barracks":
                return new Barracks(_armyUnitsInfos, _world, this);
            case "StockpileBlock":
            case "Barn":
            case "Armory":
                var stockpile = new StockpileBlock(_world, GetStorageInfo(buildingInfo));
                _world.Stockpile.AddStockpileBlock(stockpile);
                return stockpile;
            case "Cityhouse":
                return new CityHouse(_world);
            default:
                return new Workplace(_world);
        }
    }

    private StorageInfo GetStorageInfo(BuildingInfo buildingInfo)
    {
        return _worldInfo.StorageInfos.First(info => info.BuildingId == buildingInfo.Id);
    }

    private ActorBehaviour CreateBehaviour(string behaviourId)
    {
        return Activator.CreateInstance(_behaviourMap[behaviourId]) as ActorBehaviour;
    }

    private void BuildTypeMap()
    {
        var behaviours = FindDerivedTypes(typeof(TestUnitFactory).Assembly, typeof(ActorBehaviour));

        _behaviourMap = new Dictionary<string, Type>();

        foreach (var each in behaviours)
        {
            _behaviourMap[each.Name] = each;
        }

        //TODO GET UNITs TYPE from another resource
        _armyUnitsInfos.AddRange(UnitInfos.Where(x => x.Name != "Peasant"));
    }


    private void Awake()
    {
        
    }

    private IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
    {
        return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t));
    }

    #endregion
}