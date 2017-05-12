using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Actors;
using Assets.Scripts.Workplace;
using Assets.Scripts.World;
using UnityEngine;
using System;
using System.Linq;

public abstract class SelectionEventHandler
{
    public virtual bool HandleDestinationClick(Vector3 destination)
    {
        return false; //False means event wasn't handled; this helps SelectionManager understand if it should drop the selected object alltogether
    }

    public virtual bool HandleEntityClick(Entity entity)
    {
        return false;
    }
}

public class NullSelectionEventHandler : SelectionEventHandler
{

}

public class SelectionManager
{
    public event Action<Entity> EntitySelected;

    private TestWorld _world;

    private bool _isSelectingWithRectangle = false;

    private Vector2 _selectionStartingPoint;

    private List<Entity> _selectedEntities = new List<Entity>();

    public void SetWorld(TestWorld world)
    {
        _world = world;
    }

    public void SetSelectedEntity(Entity entity)
    {
        Debug.Log(entity);

        if (EntitySelected != null)
        {
            EntitySelected.Invoke(entity);
        }
    }

    public void SetSelectedEntities(IList<Entity> entities)
    {
        //Todo composite selection with compact views
    }
    public void OnGUI()
    {
        if (Event.current.type == EventType.MouseDrag)
        {
            if (!_isSelectingWithRectangle)
            {
                _selectionStartingPoint = Event.current.mousePosition;
                _isSelectingWithRectangle = true;
            }

            CheckMultiSelection(_selectionStartingPoint, Event.current.mousePosition);

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
        GUI.DrawTexture(new Rect(fromScreenPoint, toScreenPoint - fromScreenPoint), Texture2D.whiteTexture, ScaleMode.StretchToFill);
        GUI.color = Color.white;
    }

    private void CheckMultiSelection(Vector2 fromScreenPoint, Vector2 toScreenPoint)
    {
        _selectedEntities.Clear();

        var actors = _world.GetEntities().OfType<Actor>();

        var camera = Camera.main;
        var ray = camera.ScreenPointToRay(Input.mousePosition);

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
    }

    private void CheckSingleSelectionAndOrders()
    {
        var actors = _world.GetEntities();

        var camera = Camera.main;
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        var didGiveOrder = false;

        foreach (var each in actors)
        {
            var isClicked = each.GetBounds().IntersectRay(ray);
            if (isClicked)
            {
                // Debug.Log("Clicked " + each.Info);

                if (_selectedEntities.Any())
                {
                    var didHandle = false;
                    foreach (var eachActor in _selectedEntities)
                    {
                        // SetAttackTarget(eachActor.Behaviour as SoldierBehaviour, each);
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

        if (!didGiveOrder)
        {
            var distance = 0f;
            if (new Plane(Vector3.up, Vector3.zero).Raycast(ray, out distance))
            {
                var destination = ray.GetPoint(distance);

                var didHandle = false;

                foreach (var eachActor in _selectedEntities)
                {
                    // SetDestination(eachActor.Behaviour as SoldierBehaviour, destination);
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

}
