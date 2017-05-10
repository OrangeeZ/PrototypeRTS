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

    private GameWorld _world;
    private bool _isEnemy = false;
    private Dictionary<string, Type> _behaviourMap;

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

    public BuildingInfo[] BuildingInfos{get { return _buildingInfos; }}

    #endregion

    void Awake()
    {
        BuildTypeMap();
    }

    public void SetWorld(GameWorld world)
    {
        _world = world;
    }
    
    public void CreateUnit(UnitInfo unitInfo)
    {
        var unit = new Actor(_world);
        var city = _world.ActivePlayer.City;
        var unitView = Instantiate(unitInfo.Prefab);
        unitView.SetIsEnemy(_isEnemy);
        unit.SetView(unitView);
        unit.SetBehaviour(CreateBehaviour(unitInfo.BehaviourId));
        var randomPosition = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(5, 10f);
        randomPosition.y = 0;
        unit.SetPosition(city.GetFireplace().position + randomPosition);
        unit.SetInfo(unitInfo);
        unit.SetHealth(unitInfo.Hp);
        unit.SetIsEnemy(_isEnemy);
        _world.EntitiesBehaviour.AddEntity(unit);
    }

    public void CreateBuilding(BuildingInfo buildingInfo)
    {
        var building = new Workplace(_world);
        var city = _world.ActivePlayer.City;
        building.SetView(Instantiate(buildingInfo.Prefab));
		building.SetInfo(buildingInfo);

        var randomPosition = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(5, 10f);
        randomPosition.y = 0;

        building.SetPosition(city.GetFireplace().position + randomPosition);
        building.SetHealth(10);

        _world.EntitiesBehaviour.AddEntity(building);
    }

    private ActorBehaviour CreateBehaviour(string behaviourId)
    {
        Debug.Log(_behaviourMap[behaviourId]);

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
    }

    private IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
    {
        return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t));
    }
}
