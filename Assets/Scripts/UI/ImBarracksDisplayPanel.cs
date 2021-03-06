﻿using UnityEngine;

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
            GUILayout.BeginVertical();
            
            var unit = armyUnit;
            GUILayout.Label($"Unit name: {armyUnit.Name}; can hire: {barracks.CanHireUnit(armyUnit)}");
            if (GUILayout.Button("Hire Unit") && barracks.CanHireUnit(armyUnit))
            {
                barracks.HireUnit(unit);
            }
            
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }
}