using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actors;
using Assets.Scripts.World;
using UnityEngine;

public class UnitCommandModule
{
    private Actor _selectedActor;

    private TestWorld _world;

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
}
