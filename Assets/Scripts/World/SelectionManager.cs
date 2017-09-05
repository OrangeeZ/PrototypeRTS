using System.Collections.Generic;
using Assets.Scripts.World;
using UnityEngine;
using System;
using System.Linq;
using Actors;

public abstract class SelectionEventHandler
{
    public virtual bool HandleDestinationClick(Vector3 destination)
    {
        // False means event wasn't handled
        // This helps SelectionManager understand if it should drop the selected object alltogether
        return false;
    }

    public virtual bool HandleEntityClick(Entity entity)
    {
        return false;
    }
}

public class NullSelectionEventHandler : SelectionEventHandler
{
}

public class SelectionManager : IGuiDrawer
{
    public IList<Entity> SelectedEntities => _selectedEntities;

    public event Action<Entity> EntitySelected;
    public event Action SelectionUpdated;

    private BaseWorld _world;
    private bool _isSelectingWithRectangle = false;
    private Vector2 _selectionStartingPoint;
    private List<Entity> _selectedEntities = new List<Entity>();

    private float _minDragDelta;

    public SelectionManager(BaseWorld world)
    {
        _world = world;
        _minDragDelta = Mathf.Sqrt(2f);
    }

    public void Draw()
    {
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
        {
            var deltaMagnitude = Event.current.delta.magnitude;
            if (!_isSelectingWithRectangle && deltaMagnitude > _minDragDelta)
            {
                _selectionStartingPoint = Event.current.mousePosition;
                _isSelectingWithRectangle = true;
            }

            if (_isSelectingWithRectangle)
            {
                CheckMultiSelection(_selectionStartingPoint, Event.current.mousePosition);
            }

            if (_selectedEntities.Count == 1)
            {
                SetSelectedEntity(_selectedEntities.First());
            }
        }

        if (Event.current.type == EventType.MouseUp)
        {
            if (!_isSelectingWithRectangle)
            {
                CheckSingleSelectionAndOrders();
            }

            _isSelectingWithRectangle = false;
        }

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            _isSelectingWithRectangle = false;
            _selectedEntities.Clear();

            SetSelectedEntity(null);
        }

        if (_isSelectingWithRectangle)
        {
            DrawSelectionRectangle(_selectionStartingPoint, Event.current.mousePosition);
        }
    }

    private void DrawSelectionRectangle(Vector2 fromScreenPoint, Vector2 toScreenPoint)
    {
        GUI.color = new Color(0, 0, 0, 0.2f);
        GUI.DrawTexture(new Rect(fromScreenPoint, toScreenPoint - fromScreenPoint),
            Texture2D.whiteTexture, ScaleMode.StretchToFill);
        GUI.color = Color.white;
    }

    private void CheckMultiSelection(Vector2 fromScreenPoint, Vector2 toScreenPoint)
    {
        _selectedEntities.Clear();

        var actors = _world.EntityMapping.GetEntitiesByType<Actor>();

        var camera = Camera.main;

        var selectionRect = new Rect(fromScreenPoint, toScreenPoint - fromScreenPoint);

        foreach (var each in actors)
        {
            if (!(each.Behaviour is SoldierBehaviour) || each.IsEnemy)
            {
                continue;
            }

            var screenPosition = camera.WorldToScreenPoint(each.Position);
            screenPosition.y = Screen.height - screenPosition.y;

            if (selectionRect.Contains(screenPosition))
            {
                _selectedEntities.Add(each);
            }
        }

        SetSelectedEntities(_selectedEntities);
    }

    private void CheckSingleSelectionAndOrders()
    {
        var entities = _world.Entities.GetItems();
        var camera = Camera.main;
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        var didGiveOrder = false;

        foreach (var each in entities)
        {
            var isClicked = each.GetBounds().IntersectRay(ray);
            if (isClicked)
            {
                if (_selectedEntities.Any())
                {
                    var didHandle = false;
                    foreach (var eachActor in _selectedEntities)
                    {
                        didHandle = eachActor.GetSelectionEventHandler().HandleEntityClick(each) || didHandle;
                    }

                    if (!didHandle)
                    {
                        _selectedEntities.Clear();
                    }
                }
                else
                {
                    _selectedEntities.Clear();
                    _selectedEntities.Add(each);

                    SetSelectedEntity(each);
                }

                didGiveOrder = true;
            }
        }

        if (Event.current.button != 1)
        {
            return;
        }
        
        if (!didGiveOrder)
        {
            var distance = 0f;
            if (new Plane(Vector3.up, Vector3.zero).Raycast(ray, out distance))
            {
                var destination = ray.GetPoint(distance);

                var didHandle = false;

                foreach (var eachActor in _selectedEntities)
                {
                    didHandle = eachActor.GetSelectionEventHandler().HandleDestinationClick(destination) || didHandle;
                }

                if (!didHandle)
                {
                    _selectedEntities.Clear();
                    SetSelectedEntity(null);
                }
            }
        }
    }

    private void SetSelectedEntity(Entity entity)
    {
        EntitySelected?.Invoke(entity);
        SelectionUpdated?.Invoke();
    }

    private void SetSelectedEntities(IList<Entity> entities)
    {
        //Todo composite selection with compact views

        SelectionUpdated?.Invoke();
    }
}