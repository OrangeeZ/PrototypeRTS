using Assets.Scripts.Workplace;
using UnityEngine;

public class ImBarracksDisplayPanel : EntityDisplayPanel
{
    public override void DrawOnGUI()
    {
		var barracks = Entity as Barracks;
        if (barracks == null)
            return;
        var armyUnits = barracks.AvailableUnits;
        GUILayout.BeginHorizontal();
        foreach (var armyUnit in armyUnits)
        {
            GUILayout.BeginVertical("Cell");
            var unit = armyUnit;
            GUILayout.Label(string.Format("Unit name : {0}",armyUnit.Name));
            if (GUILayout.Button("Hire Unit"))
            {
                barracks.HireArmyUnit(unit);
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }
}
