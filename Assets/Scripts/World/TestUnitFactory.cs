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
    private BaseWorld _world;
    private Dictionary<string, Type> _behaviourMap;
    private List<UnitInfo> _armyUnitsInfos = new List<UnitInfo>();
    private WorldData _worldData;

    public byte FactionId;
    public UnitInfo[] UnitInfos => _worldData.UnitInfos;
    public BuildingInfo[] BuildingInfos => _worldData.BuildingInfos;

    public void Initialize(BaseWorld world, WorldData worldData)
    {
        _world = world;
        _worldData = worldData;
        BuildTypeMap();
    }

    public Entity CreateUnit(UnitInfo unitInfo,bool ignoreLimit = false)
    {
        //no free citizen or population limit reached
        if (_world.Population >= _world.MaxPopulation && !ignoreLimit)
        {
            return null;
        }

        var unit = new Actor(_world);
        var result = CreateUnit(unitInfo, unit);

        var randomPosition = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(5, 10f);
        randomPosition.y = 0;
        unit.SetPosition(_world.GetFireplace() + randomPosition);

        _world.Entities.Add(result);

        return result;
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
        return _worldData.StorageInfos.First(info => info.BuildingId == buildingInfo.Id);
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