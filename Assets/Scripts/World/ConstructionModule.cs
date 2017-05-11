using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.World;
using UnityEngine;

public class ConstructionModule
{
    private TestUnitFactory _unitFactory;

    private GameObject _selectedBuilding;

    private BuildingInfo _selectedBuildingInfo;

    private bool _isPlacingBuilding = false;

    public void SetUnitFactory(TestUnitFactory unitFactory)
    {
        _unitFactory = unitFactory;
    }

    public void Update(float deltaTime)
    {
        if (_selectedBuilding != null)
        {
            var camera = Camera.main;
            var ray = camera.ScreenPointToRay(Input.mousePosition);
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
        }
        else
        {
            if (Event.current.keyCode == KeyCode.Escape)
            {
                Object.Destroy(_selectedBuilding);

                _isPlacingBuilding = false;
            }
        }

		GUILayout.EndVertical();

        GUILayout.EndArea();
    }
}
