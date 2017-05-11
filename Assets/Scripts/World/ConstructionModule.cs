using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Workplace;
using Assets.Scripts.World;
using UnityEngine;

public class ConstructionModule
{
    private TestWorld _world;

    private TestUnitFactory _unitFactory;

    private GameObject _selectedBuilding;

    private BuildingInfo _selectedBuildingInfo;

    private bool _isPlacingBuilding = false;

    private bool _isRemovingBuildings = false;

    public void SetWorld(TestWorld world)
    {
        _world = world;
    }

    public void SetUnitFactory(TestUnitFactory unitFactory)
    {
        _unitFactory = unitFactory;
    }

    public void Update(float deltaTime)
    {
        var camera = Camera.main;
        var ray = camera.ScreenPointToRay(Input.mousePosition);

        if (_selectedBuilding != null)
        {
            var plane = new Plane(Vector3.up, Vector3.zero);

            var distance = 0f;

            if (plane.Raycast(ray, out distance))
            {
                var buildingPosition = ray.GetPoint(distance);

                _selectedBuilding.transform.position = buildingPosition;
            }

            if (Input.GetMouseButtonDown(0))
            {
                _isPlacingBuilding = false;
                Object.Destroy(_selectedBuilding);

                var entity = _unitFactory.CreateBuilding(_selectedBuildingInfo);
                entity.SetPosition(_selectedBuilding.transform.position);
            }
        }

        if (_isRemovingBuildings && Input.GetMouseButtonDown(0))
        {
            var entities = _world.GetEntities().Where(_ => _ is Workplace);

            foreach (var each in entities)
            {
                if (each.GetBounds().IntersectRay(ray))
                {
                    each.DealDamage(each.Health); //Will later be changed into a disband command if for some reason we want this to work with soldiers
                }
            }
        }
    }

    public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, Screen.height - 100, Screen.width, 100));
        GUILayout.Label("Construction module");

        GUILayout.BeginHorizontal();

        if (!_isPlacingBuilding)
        {
            foreach (var each in _unitFactory.BuildingInfos)
            {
                if (GUILayout.Button(each.Name, GUILayout.ExpandWidth(false)))
                {
                    _isPlacingBuilding = true;
                    _selectedBuilding = Object.Instantiate(each.Prefab).gameObject;
                    _selectedBuildingInfo = each;

                    break;
                }
            }

            if (GUILayout.Button(_isRemovingBuildings ? "Stop removing buildings" : "Start removing buildings", GUILayout.ExpandWidth(false)))
            {
                _isRemovingBuildings = !_isRemovingBuildings;
            }
        }

        if (Event.current.keyCode == KeyCode.Escape)
        {
            Object.Destroy(_selectedBuilding);

            _isPlacingBuilding = false;
            _isRemovingBuildings = false;
        }

        GUILayout.EndVertical();

        GUILayout.EndArea();
    }
}
