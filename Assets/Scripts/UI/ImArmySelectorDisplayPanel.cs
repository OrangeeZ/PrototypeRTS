﻿using System.Collections.Generic;
using System.Linq;
using Actors;
using UnityEngine;

public class ImArmySelectorDisplayPanel : EntityDisplayPanel
{
    private Dictionary<UnitInfo, Actor[]> _groupedUnits = new Dictionary<UnitInfo, Actor[]>();

    public override void Show()
    {
        SelectionManager.SelectionUpdated += UpdateGroupedUnits;
    }

    public override void Hide()
    {
        SelectionManager.SelectionUpdated -= UpdateGroupedUnits;
    }

    public override void DrawOnGUI()
    {
        using (new GUILayout.HorizontalScope())
        {
            using (new GUILayout.VerticalScope(GUILayout.Width(100)))
            {
                if (GUILayout.Button("Aggressive"))
                {
                    SetSoldierBehaviourMode(SoldierBehaviour.BehaviourMode.Aggressive);
                }

                if (GUILayout.Button("Passive"))
                {
                    SetSoldierBehaviourMode(SoldierBehaviour.BehaviourMode.Passive);
                }

                if (GUILayout.Button("Defensive"))
                {
                    SetSoldierBehaviourMode(SoldierBehaviour.BehaviourMode.Defensive);
                }
            }

            foreach (var each in _groupedUnits)
            {
                using (new GUILayout.VerticalScope(GUILayout.Width(100)))
                {
                    GUILayout.Label(each.Key.Name);
                    GUILayout.Label($"Count: {each.Value.Length}");
                }
            }
        }
    }

    private void UpdateGroupedUnits()
    {
        var selectedEntities = SelectionManager.SelectedEntities;
        var selectedUnits = selectedEntities.OfType<Actor>();

        _groupedUnits = selectedUnits.GroupBy(_ => _.Info, _ => _).ToDictionary(_ => _.Key, _ => _.ToArray());
    }

    private void SetSoldierBehaviourMode(SoldierBehaviour.BehaviourMode targetBehaviourMode)
    {
        var soldierBehaviours = _groupedUnits.Values
            .SelectMany(_ => _)
            .Select(_ => _.Behaviour)
            .OfType<SoldierBehaviour>();

        foreach (var each in soldierBehaviours)
        {
            each.SetBehaviourMode(targetBehaviourMode);
        }
    }
}