using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actors;
using Assets.Scripts.Behaviour;
using UnityEngine;

public class SoldierBehaviour : ActorBehaviour
{
    private class Target
    {
        public Vector3 TargetPosition;
        public Entity TargetEntity;
        public float StoppingDistance;

        public Vector3 GetDestination()
        {
            return TargetEntity != null ? TargetEntity.Position : TargetPosition;
        }
    }

    private Target _target;

    public void SetDestination(Vector3 destination)
    {
        _target = new Target();

        _target.TargetPosition = destination;
        _target.StoppingDistance = 0.1f;
    }

    public void SetAttackTarget(Entity target)
    {
        _target = new Target();

        _target.TargetEntity = target;
        _target.StoppingDistance = Actor.Info.AttackRange;
    }

    protected override IEnumerator UpdateRoutine()
    {
        while (true)
        {
            if (!IsTargetValid())
            {
                _target = null;
            }

            var navAgent = Actor.NavAgent;

            navAgent.isStopped = true;

            while (_target == null)
            {
                FindAttackTarget();

                yield return null;
            }

            navAgent.isStopped = false;

            navAgent.SetDestination(_target.GetDestination());
            navAgent.stoppingDistance = Actor.Info.AttackRange - 0.5f;
            while (navAgent != null && (!navAgent.hasPath || navAgent.remainingDistance > _target.StoppingDistance))
            {
                yield return null;

                navAgent.SetDestination(_target.GetDestination());

                if (!IsTargetValid())
                {
                    break;
                }

                yield return null;
            }

            if (navAgent == null)
            {
                yield break;
            }

            if (_target.TargetEntity != null)
            {
                if (Vector3.Distance(_target.GetDestination(), Actor.Position) < _target.StoppingDistance)
                {
                    _target.TargetEntity.DealDamage(Actor.Info.AttackStrength);

                    var delay = (float)Actor.Info.AttackSpeed;
                    while (delay > 0)
                    {
                        delay -= DeltaTime;

                        yield return null;
                    }
                }
            }
            else
            {
                _target = null;
            }

            yield return null;
        }
    }

    private void FindAttackTarget()
    {
        var world = Actor.World;
        var entities = world.Entities;
        var potentialTargets = entities.GetItems()
            .Where(_ => _.IsEnemy != Actor.IsEnemy && _ != Actor);
        foreach (var each in potentialTargets)
        {
            if (Vector3.Distance(each.Position, Actor.Position) <= Actor.Info.AttackRange)
            {
                SetAttackTarget(each);

                return;
            }
        }
    }

    private bool IsTargetValid()
    {
        if (_target == null)
        {
            return false;
        }

        if (_target.TargetEntity != null)
        {
            return _target.TargetEntity.Health > 0;
        }

        return true;
    }
}
