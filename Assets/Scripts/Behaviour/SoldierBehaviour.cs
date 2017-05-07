using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actors;
using Assets.Scripts.Behaviour;
using UnityEngine;

public class SoldierBehaviour : ActorBehaviour
{
    private Entity _target;

    public void SetTarget(Actor target)
    {
        _target = target;
    }

    protected override IEnumerator UpdateRoutine()
    {
        while (true)
        {
            if (_target != null && _target.Health <= 0)
            {
                _target = null;
            }

            var navAgent = Actor.NavAgent;

            navAgent.isStopped = true;

            while (_target == null)
            {
                _target = GetAnyEnemy();

                yield return null;
            }

            navAgent.isStopped = false;

            navAgent.SetDestination(_target.Position);
            while (navAgent != null && (!navAgent.hasPath || navAgent.remainingDistance > 1f))
            {
                yield return null;
                yield return null;
                yield return null;

                navAgent.SetDestination(_target.Position);

                if (_target.Health <= 0)
                {
                    break;
                }

                yield return null;
            }

            if (navAgent == null)
            {
                yield break;
            }

            if (Vector3.Distance(_target.Position, Actor.Position) < 1f)
            {
                _target.DealDamage(1);

                var delay = 1f;
                while (delay > 0)
                {
                    delay -= DeltaTime;

                    yield return null;
                }
            }

            yield return null;
        }
    }

    private Entity GetAnyEnemy()
    {
        var world = Actor.World;
        var entities = world.GetEntities();

        return entities.FirstOrDefault(_ => _.IsEnemy != Actor.IsEnemy && _ != Actor);
    }
}
