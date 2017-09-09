using System.Linq;
using Actors;
using Assets.Scripts.World;
using UnityEngine;

public class ImUiController : IGuiDrawer
{
    private EntityDisplayPanel _defaultDisplayPanel;

    private EntityDisplayPanel _currentDisplayPanel;
    private readonly SelectionManager _selectionManager;

    public ImUiController(SelectionManager selectionManager, WorldData worldData)
    {
        _defaultDisplayPanel = Object.Instantiate(worldData.DefaultDisplayPanel);
        _defaultDisplayPanel.Initialize(selectionManager, null);

        _selectionManager = selectionManager;
        _selectionManager.SelectionUpdated += OnSelectionUpdated;
    }

    public void OnSelectionUpdated()
    {
        ClearEntityDisplayPanel();

        var entity = default(Entity);
        
        if (_selectionManager.SelectedEntities.Count == 1)
        {
            entity = _selectionManager.SelectedEntities.First();
        }

        if (entity != null && entity.GetDisplayPanelPrefab() != null)
        {
            _currentDisplayPanel = Object.Instantiate(entity.GetDisplayPanelPrefab());
            _currentDisplayPanel.Initialize(_selectionManager, entity);
            _currentDisplayPanel.Show();
        }
        
        UpdateCurrentDisplayPanel(_currentDisplayPanel);
    }

    public void ClearEntityDisplayPanel()
    {
        if (_currentDisplayPanel != null)
        {
            _currentDisplayPanel.Hide();
            Object.Destroy(_currentDisplayPanel.gameObject);
        }
    }

    public void UpdateCurrentDisplayPanel(EntityDisplayPanel displayPanel)
    {
        if (displayPanel == null)
        {
            if (!_defaultDisplayPanel.gameObject.activeInHierarchy)
            {
                _defaultDisplayPanel.gameObject.SetActive(true);
                _defaultDisplayPanel.Show();
            }
        }
        else
        {
            if (_defaultDisplayPanel.gameObject.activeInHierarchy)
            {
                _defaultDisplayPanel.Hide();
                _defaultDisplayPanel.gameObject.SetActive(false);
            }
        }
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
        else
        {
            _defaultDisplayPanel.DrawOnGUI();
        }

        GUILayout.EndArea();
    }
}