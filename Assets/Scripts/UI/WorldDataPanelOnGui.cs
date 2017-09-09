using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDataPanelOnGui : IGuiDrawer {

    private readonly BaseWorld _world;
    private string _debt = String.Empty;

    public WorldDataPanelOnGui(BaseWorld world)
    {
        _world = world;
    }

    public void Draw()
    {
        GUILayout.BeginVertical();
        GUILayout.Label(string.Format("MaxPopulation : {0}", _world.MaxPopulation));
        GUILayout.Label(string.Format("Population : {0}", _world.Population));
        GUILayout.Label(string.Format("Gold : {0}", _world.Gold));

        GUILayout.BeginHorizontal();

        GUILayout.Label(string.Format("Tax : {0}", _world.Tax));
        _debt = GUILayout.TextField(_debt);
        if (GUILayout.Button("SET"))
        {
            int value;
            if (int.TryParse(_debt, out value))
            {
                _world.Tax = value;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Label(string.Format("FreeCitizensCount : {0}", _world.FreeCitizensCount));
        GUILayout.EndVertical();
    }
    
}
