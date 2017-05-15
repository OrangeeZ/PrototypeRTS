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
    [SerializeField]
    private UnitInfo[] _unitInfos;
    
    [SerializeField]
    private BuildingInfo[] _buildingInfos;

    private BaseWorld _world;
    private bool _isEnemy = false;
    private Dictionary<string, Type> _behaviourMap;
    private List<UnitInfo> _armyUnitsInfos = new List<UnitInfo>();

    #region public properties

    public bool IsEnemy
    {
        get
        {
            return _isEnemy;
        }
        set
        {
            _isEnemy = value;
        }
    }

    public UnitInfo[] UnitInfos { get { return _unitInfos; } }

    public BuildingInfo[] BuildingInfos { get { return _buildingInfos; } }

    #endregion

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
        var unit = new Actor(_world);
        var city = _world;
        var unitView = Instantiate(unitInfo.Prefab);
        
        unitView.SetIsEnemy(_isEnemy);
        unit.SetView(unitView);
        unit.SetBehaviour(CreateBehaviour(unitInfo.BehaviourId));
        
        var randomPosition = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(5, 10f);
        randomPosition.y = 0;
        unit.SetPosition(_world.GetFireplace() + randomPosition);
        unit.SetInfo(unitInfo);
        unit.SetHealth(unitInfo.Hp);
        unit.SetIsEnemy(_isEnemy);
        _world.Entities.Add(unit);
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
        switch (buildingInfo.Name)
        {
            case "Barracks":
                return new Barracks(_armyUnitsInfos,_world,this);
            case "Stockpile":
                return new StockpileBlock(_world);
            default:
                return new Workplace(_world);
        }
    }

    private ActorBehaviour CreateBehaviour(string behaviourId)
    {
        return System.Activator.CreateInstance(_behaviourMap[behaviourId]) as ActorBehaviour;
    }

    private void BuildTypeMap()
    {
        var behaviours = FindDerivedTypes(typeof(TestUnitFactory).Assembly, typeof(ActorBehaviour));

        _behaviourMap = new Dictionary<string, Type>();

        foreach (var each in behaviours)
        {
            Debug.Log(each.Name);
            _behaviourMap[each.Name] = each;
        }
        //TODO GET UNITs TYPE from another resource
        _armyUnitsInfos.AddRange(_unitInfos.Where(x=>x.Name!= "Peasant"));
    }

    private IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
    {
        return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t));
    }
}
