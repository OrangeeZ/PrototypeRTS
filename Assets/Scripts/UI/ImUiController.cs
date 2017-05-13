using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Actors;
using Assets.Scripts.World;
using UnityEngine;

public class ImUiController : IGuiDrawer
{
    private EntityDisplayPanel _currentDisplayPanel;
    private SelectionManager _selectionManager;
    private BaseWorld _world;

    public ImUiController(BaseWorld world, SelectionManager selectionManager)
    {
        _world = world;
        _selectionManager = selectionManager;
        _selectionManager.EntitySelected += SetEntityDisplayPanelInfo;
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

    public void Draw()
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
