using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actors;
using UniRx;
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
            foreach (var each in _groupedUnits)
            {
                using (new GUILayout.VerticalScope(GUILayout.Width(200)))
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
}