using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Actors;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Workplace;
using UnityEngine;

public class TestUnitFactory : MonoBehaviour
{
    public bool IsEnemy { get; set; } = false;

    public UnitInfo[] UnitInfos => _unitInfos;

    public BuildingInfo[] BuildingInfos => _buildingInfos;

    [SerializeField]
    private UnitInfo[] _unitInfos;

    [SerializeField]
    private BuildingInfo[] _buildingInfos;

    private BaseWorld _world;
    private Dictionary<string, Type> _behaviourMap;
    private List<UnitInfo> _armyUnitsInfos = new List<UnitInfo>();

    void Awake()
    {
        BuildTypeMap();
    }

    public void SetWorld(BaseWorld world)
    {
        _world = world;
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

        unitView.SetIsEnemy(IsEnemy);
        unit.SetView(unitView);
        unit.SetBehaviour(CreateBehaviour(unitInfo.BehaviourId));

        unit.SetInfo(unitInfo);
        unit.SetHealth(unitInfo.Hp);
        unit.SetIsEnemy(IsEnemy);

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

    private Building CreateBuildingEntity(BuildingInfo buildingInfo)
    {
        switch (buildingInfo.Id)
        {
            case "Barracks":
                return new Barracks(_armyUnitsInfos, _world, this);
            case "StockpileBlock":
                return new StockpileBlock(_world);
            case "Cityhouse":
                return new CityHouse(_world);
            default:
                return new Workplace(_world);
        }
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
        _armyUnitsInfos.AddRange(_unitInfos.Where(x => x.Name != "Peasant"));
    }

    private IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
    {
        return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t));
    }

    [ContextMenu("Hook data")]
    private void HookData()
    {
        _unitInfos = UnityEditor.AssetDatabase
            .FindAssets("t:unitinfo")
            .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
            .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<UnitInfo>)
            .ToArray();

        _buildingInfos = UnityEditor.AssetDatabase
            .FindAssets("t:buildinginfo")
            .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
            .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<BuildingInfo>)
            .ToArray();
    }
}