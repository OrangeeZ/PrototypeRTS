using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Actors;
using Assets.Scripts.World;
using UnityEngine;

public class ImUiController
{
    private EntityDisplayPanel _currentDisplayPanel;
    private TestWorld _world;

    public void SetWorld(TestWorld world)
    {
        _world = world;

        _world.SelectionManager.EntitySelected += SetEntityDisplayPanelInfo;
    }

    public void SetEntityDisplayPanelInfo(Entity entity)
    {
        ClearEntityDisplayPanel();

        if (entity != null && entity.GetDisplayPanelPrefab() != null)
        {
            _currentDisplayPanel = UnityEngine.Object.Instantiate(entity.GetDisplayPanelPrefab());
            _currentDisplayPanel.SetEntity(entity);
        }
    }

    public void ClearEntityDisplayPanel()
    {
        UnityEngine.Object.Destroy(_currentDisplayPanel);
    }

    public void OnGUI()
    {
        var panelWidth = Screen.width / 2;
        var panelHeight = 100;
        var screenCenter = Screen.width / 2;

        var rect = new Rect(screenCenter - panelWidth / 2, Screen.height - panelHeight, panelWidth, panelHeight);

        GUI.color = new Color(0, 0, 0, 0.5f);
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = Color.white;

        GUILayout.BeginArea(rect);

        if (_currentDisplayPanel != null)
        {
            _currentDisplayPanel.DrawOnGUI();
        }

        GUILayout.EndArea();
    }
}
