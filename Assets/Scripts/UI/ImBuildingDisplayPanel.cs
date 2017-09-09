using Buildings;
using UnityEngine;

public class ImBuildingDisplayPanel : EntityDisplayPanel
{
    private const string Stop = "STOP";
    private const string Activate = "ACTIVATE";
    private GUIStyle _selectedButtonStyle;

    public override void DrawOnGUI()
    {
        if (_selectedButtonStyle == null)
        {
            _selectedButtonStyle = new GUIStyle(GUI.skin.button);
            _selectedButtonStyle.normal.textColor = Color.red;
        }
        var workplace = Entity as Workplace;
        if (workplace == null)
        {
            return;
        }
        GUILayout.Label(workplace.Info.Name);
        GUILayout.BeginHorizontal();
        DrawStatus(workplace);
        if(workplace.AvailableProduction.Count>0)
            DrawProduction(workplace);
        GUILayout.EndHorizontal();
    }

    private void DrawProduction(Building building)
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Production");
        for (int i = 0; i < building.AvailableProduction.Count; i++)
        {
            var item = building.AvailableProduction[i];
            if (item == building.ActiveProductionCycle ?
                GUILayout.Button(item.Name,_selectedButtonStyle) : GUILayout.Button(item.Name))
            {
                building.ActivateProduction(item);
            }
        }
        GUILayout.EndVertical();
    }

    private void DrawStatus(Workplace workplace)
    {
        GUILayout.BeginVertical();
        if (GUILayout.Button("REMOVE"))
            workplace.Kill();
        if (GUILayout.Button(workplace.IsActive ? Stop : Activate))
            workplace.SetState(!workplace.IsActive);
        GUILayout.EndVertical();
    }
}