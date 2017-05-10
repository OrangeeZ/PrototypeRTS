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
			navAgent.stoppingDistance = Actor.Info.AttackRange - 0.5f;
            while (navAgent != null && (!navAgent.hasPath || navAgent.remainingDistance > Actor.Info.AttackRange))
            {
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

            if (Vector3.Distance(_target.Position, Actor.Position) < Actor.Info.AttackRange)
            {
                _target.DealDamage(Actor.Info.AttackStrength);

                var delay = (float)Actor.Info.AttackSpeed;
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
        var entities = world.EntitiesBehaviour.GetEntities();

        return entities.FirstOrDefault(_ => _.IsEnemy != Actor.IsEnemy && _ != Actor);
    }
}
