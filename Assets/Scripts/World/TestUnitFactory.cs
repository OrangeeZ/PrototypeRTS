using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Actors;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Workplace;
using Assets.Scripts.World;
using UnityEngine;

public class TestUnitFactory : MonoBehaviour
{
    [SerializeField]
    private UnitInfo[] _unitInfos;

    [SerializeField]
    private BuildingInfo[] _buildingInfos;

    private TestWorld _world;

    private bool _isEnemy = false;

    private Dictionary<string, Type> _behaviourMap;

    void Awake()
    {
        BuildTypeMap();
    }

    public void SetWorld(TestWorld world)
    {
        _world = world;
    }

    public void DrawMenu()
    {
        GUILayout.Space(10);

        _isEnemy = GUILayout.Toggle(_isEnemy, "Is Enemy");

        foreach (var each in _unitInfos)
        {
            if (GUILayout.Button("Create " + each.Name))
            {
                CreateUnit(each);
            }
        }

        GUILayout.Space(10);

        foreach (var each in _buildingInfos)
        {
            if (GUILayout.Button("Create " + each.Name))
            {
                CreateBuilding(each);
            }
        }
    }

    public void CreateUnit(UnitInfo unitInfo)
    {
        var unit = new Actor(_world);

        var unitView = Instantiate(unitInfo.Prefab);
        unitView.SetIsEnemy(_isEnemy);
        unit.SetView(unitView);

        unit.SetBehaviour(CreateBehaviour(unitInfo.BehaviourId));

        var randomPosition = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(5, 10f);
        randomPosition.y = 0;

        unit.SetPosition(_world.GetFireplace().position + randomPosition);
        unit.SetInfo(unitInfo);

        unit.SetHealth(unitInfo.Hp);
        unit.SetIsEnemy(_isEnemy);

        _world.AddEntity(unit);
    }

    public void CreateBuilding(BuildingInfo buildingInfo)
    {
        var building = new Workplace(_world);

        building.SetView(Instantiate(buildingInfo.Prefab));
		building.SetInfo(buildingInfo);

        var randomPosition = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(5, 10f);
        randomPosition.y = 0;

        building.SetPosition(_world.GetFireplace().position + randomPosition);
        building.SetHealth(10);

        _world.AddEntity(building);
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
