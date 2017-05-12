using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actors;
using Assets.Scripts.World;
using UnityEngine;

public class UnitCommandModule
{
    private Actor _selectedActor;

    private List<Actor> _selectedActors = new List<Actor>();

    private TestWorld _world;

    private bool _isSelectingWithRectangle = false;

    private Vector2 _selectionStartingPoint;

    public void SetWorld(TestWorld world)
    {
        _world = world;
    }

    public void SetDestination(SoldierBehaviour behaviour, Vector3 destination)
    {
        behaviour.SetDestination(destination);
    }

    public void SetAttackTarget(SoldierBehaviour behaviour, Actor target)
    {
        behaviour.SetTarget(target);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var actors = _world.GetEntities().OfType<Actor>();

            var camera = Camera.main;
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            var didGiveOrder = false;

            foreach (var each in actors)
            {
                if (!(each.Behaviour is SoldierBehaviour))
                {
                    continue;
                }

                var isClicked = each.GetBounds().IntersectRay(ray);
                if (isClicked)
                {
                    Debug.Log("Clicked " + each.Info);

                    if (_selectedActor != null)
                    {
                        SetAttackTarget(_selectedActor.Behaviour as SoldierBehaviour, each);
                    }
                    else if (_selectedActors.Any())
                    {
                        foreach (var eachActor in _selectedActors)
                        {
                            SetAttackTarget(eachActor.Behaviour as SoldierBehaviour, each);
                        }
                    }
                    else
                    {
                        _selectedActor = each;
                    }

                    didGiveOrder = true;
                }
            }

            if (!didGiveOrder && _selectedActor != null)
            {
                var distance = 0f;
                if (new Plane(Vector3.up, Vector3.zero).Raycast(ray, out distance))
                {
                    var destination = ray.GetPoint(distance);
                    SetDestination(_selectedActor.Behaviour as SoldierBehaviour, destination);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _selectedActor = null;
        }
    }

    public void OnGUI()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            if (_selectedActor == null)
            {
                _isSelectingWithRectangle = true;
                _selectionStartingPoint = Event.current.mousePosition;
            }
        }

        if (Event.current.type == EventType.MouseDrag)
        {
            if (_isSelectingWithRectangle)
            {
                FindSelectedUnits(_selectionStartingPoint, Event.current.mousePosition);
            }
        }

        if (Event.current.type == EventType.MouseUp)
        {
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
            if (selectionRect.Contains(screenPosition))
            {
                _selectedActors.Add(each);
            }
        }
    }
}
