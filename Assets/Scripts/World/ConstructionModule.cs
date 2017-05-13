using System.Linq;
using Assets.Scripts.Workplace;
using UnityEngine;

public class ConstructionModule : WorldEvent,IGuiDrawer
{
    private BaseWorld _world;

    private TestUnitFactory _unitFactory;

    private GameObject _selectedBuilding;

    private BuildingInfo _selectedBuildingInfo;

    private bool _isPlacingBuilding = false;

    private bool _isRemovingBuildings = false;


    public ConstructionModule(BaseWorld world, TestUnitFactory unitFactory) : base(world)
    {
        _unitFactory = unitFactory;
    }

    public override void Update(float deltaTime)
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

        if (_isRemovingBuildings && Input.GetMouseButtonDown(0) && _world!=null)
        {
            var entities = _world.Entities.GetItems().OfType<Workplace>();
            foreach (var each in entities)
            {
                if (each.GetBounds().IntersectRay(ray))
                {
                    //Will later be changed into a disband command 
                    //if for some reason we want this to work with soldiers
                    each.DealDamage(each.Health); 
                }
            }
        }
    }

    public void Draw()
    {
        GUILayout.BeginArea(new Rect(0, Screen.height - 200, 100, 200));
        GUILayout.Label("Construction module");
        GUILayout.BeginVertical();

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

            if (GUILayout.Button(_isRemovingBuildings ? "Stop removing buildings" : 
                "Start removing buildings", GUILayout.ExpandWidth(false)))
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
