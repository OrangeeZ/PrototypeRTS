using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actors;
using Assets.Scripts.World;
using UnityEngine;

public class UnitCommandModule : IGuiDrawer
{
    private List<Actor> _selectedActors = new List<Actor>();

    private BaseWorld _world;

    private bool _isSelectingWithRectangle = false;

    private Vector2 _selectionStartingPoint;

    public void SetWorld(BaseWorld world)
    {
        _world = world;
    }

    public void SetDestination(SoldierBehaviour behaviour, Vector3 destination)
    {
        behaviour.SetDestination(destination);
    }

    public void SetAttackTarget(SoldierBehaviour behaviour, Actor target)
    {
        if (behaviour != null)
        {
            behaviour.SetAttackTarget(target);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _selectedActors.Clear();
        }
    }

    public void Draw()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            if (_selectedActors.IsEmpty())
            {
                _isSelectingWithRectangle = true;
                _selectionStartingPoint = Event.current.mousePosition;
            }
        }

        if (Event.current.type == EventType.MouseDrag)
        {
            if (!_isSelectingWithRectangle)
            {
                _selectionStartingPoint = Event.current.mousePosition;
                _isSelectingWithRectangle = true;
            }

            FindSelectedUnits(_selectionStartingPoint, Event.current.mousePosition);
        }

        if (Event.current.type == EventType.MouseUp)
        {
            if (!_isSelectingWithRectangle)
            {
                CheckSelectionAndOrders();
            }

            _isSelectingWithRectangle = false;
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

    private void FindSelectedUnits(Vector2 fromScreenPoint, Vector2 toScreenPoint)
    {
        _selectedActors.Clear();

        var actors = _world.Entities.GetItems().OfType<Actor>();

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
                _selectedActors.Add(each);
            }
        }
    }

    private void CheckSelectionAndOrders()
    {
        var actors = _world.Entities.GetItems().OfType<Actor>();

        var camera = Camera.main;
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        var didGiveOrder = false;

        foreach (var each in actors)
        {
            var isClicked = each.GetBounds().IntersectRay(ray);
            if (isClicked)
            {
                Debug.Log("Clicked " + each.Info);

                if (_selectedActors.Any())
                {
                    foreach (var eachActor in _selectedActors)
                    {
                        SetAttackTarget(eachActor.Behaviour as SoldierBehaviour, each);
                    }
                }
                else
                {
                    if (each.Behaviour is SoldierBehaviour && !each.IsEnemy)
                    {
                        _selectedActors.Clear();
                        _selectedActors.Add(each);
                    }
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

                foreach (var eachActor in _selectedActors)
                {
                    SetDestination(eachActor.Behaviour as SoldierBehaviour, destination);
                }
            }
        }
    }
}
