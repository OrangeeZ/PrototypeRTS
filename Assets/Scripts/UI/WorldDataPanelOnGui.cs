using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDataPanelOnGui : IGuiDrawer
{
    private readonly BaseWorld _world;

    public WorldDataPanelOnGui(BaseWorld world)
    {
        _world = world;
    }

    public void Draw()
    {
        GUILayout.BeginVertical();
        GUILayout.Label($"MaxPopulation : {_world.MaxPopulation}");
        GUILayout.Label($"Population : {_world.Population}");
        GUILayout.Label($"Gold : {_world.Gold}");

        using (new GUILayout.HorizontalScope())
        {
            var taxOffset = 0;
            if (GUILayout.Button("-"))
            {
                taxOffset = -1;
            }
            GUILayout.Label($"Tax : {_world.TaxController.Amount}");

            if (GUILayout.Button("+"))
            {
                taxOffset = 1;
            }
            _world.TaxController.OffsetValueIndex(taxOffset);
        }

        GUILayout.Label($"FreeCitizensCount : {_world.FreeCitizensCount}");
        GUILayout.EndVertical();
    }
}