using System.Linq;
using Assets.Scripts.Workplace;
using UnityEngine;

public class ConstructionModule : WorldEvent
{
    public GameObject SelectedBuilding { get; set; }
    public BuildingInfo SelectedBuildingInfo { get; set; }
    public bool IsPlacingBuilding { get; set; }
    public bool IsRemovingBuildings { get; set; }

    private BaseWorld _world;
    private TestUnitFactory _unitFactory;

    public ConstructionModule(BaseWorld world, TestUnitFactory unitFactory) : base(world)
    {
        _unitFactory = unitFactory;
    }

    public override void Update(float deltaTime)
    {
        var camera = Camera.main;
        var ray = camera.ScreenPointToRay(Input.mousePosition);

        if (SelectedBuilding != null)
        {
            var plane = new Plane(Vector3.up, Vector3.zero);

            var distance = 0f;

            if (plane.Raycast(ray, out distance))
            {
                var buildingPosition = ray.GetPoint(distance);

                SelectedBuilding.transform.position = buildingPosition;
            }

            if (Input.GetMouseButtonDown(0))
            {
                IsPlacingBuilding = false;
                Object.Destroy(SelectedBuilding);

                var entity = _unitFactory.CreateBuilding(SelectedBuildingInfo);
                entity.SetPosition(SelectedBuilding.transform.position);
            }
        }

        if (IsRemovingBuildings && Input.GetMouseButtonDown(0) && _world != null)
        {
            var entities = _world.Entities.GetItems().OfType<Workplace>();
            foreach (var each in entities)
            {
                if (each.GetBounds().IntersectRay(ray))
                {
                    //Will later be changed into a disband command 
                    //if for some reason we want this to work with soldiers
                    each.DealDamage(each.Health, null);
                }
            }
        }
    }
}