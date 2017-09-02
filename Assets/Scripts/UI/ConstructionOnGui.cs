using UnityEngine;

public class ConstructionOnGui : IGuiDrawer
{
    private readonly TestUnitFactory _unitFactory;
    private readonly ConstructionModule _constructionModule;

    public ConstructionOnGui(TestUnitFactory unitFactory, ConstructionModule constructionModule)
    {
        _unitFactory = unitFactory;
        _constructionModule = constructionModule;
    }

    public void Draw()
    {
        GUILayout.FlexibleSpace();
        GUILayout.Label("Construction module");
        GUILayout.BeginVertical();

        if (!_constructionModule.IsPlacingBuilding)
        {
            GUILayout.BeginVertical();
            foreach (var each in _unitFactory.BuildingInfos)
            {
                if (GUILayout.Button(each.Name, GUILayout.ExpandWidth(false)))
                {
                    _constructionModule.IsPlacingBuilding = true;
                    _constructionModule.SelectedBuilding = Object.Instantiate(each.Prefab).gameObject;
                    _constructionModule.SelectedBuildingInfo = each;
                    break;
                }
            }
            GUILayout.EndVertical();

            if (GUILayout.Button(_constructionModule.IsRemovingBuildings ? 
                "SwitchActiveState removing buildings" :
                "Start removing buildings", GUILayout.ExpandWidth(false)))
            {
                _constructionModule.IsRemovingBuildings = !_constructionModule.IsRemovingBuildings;
            }
        }

        if (Event.current.keyCode == KeyCode.Escape)
        {
            _constructionModule.IsPlacingBuilding = false;
            _constructionModule.IsRemovingBuildings = false;
            Object.Destroy(_constructionModule.SelectedBuilding);

        }

        GUILayout.EndVertical();
    }
}
