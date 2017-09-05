using System.Collections;
using System.Linq;
using Actors;
using Behaviour;
using UnityEngine;
using World.SocialModule;

public class SoldierBehaviour : ActorBehaviour
{
    public enum BehaviourMode
    {
        Aggressive,
        Passive,
        Defensive
    }

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
    private BehaviourMode _behaviourMode = BehaviourMode.Aggressive;

    public void SetDestination(Vector3 destination)
    {
        _target = new Target();

        _target.TargetPosition = destination;
        _target.StoppingDistance = 1f;
    }

    public void SetAttackTarget(Entity target)
    {
        _target = new Target();

        _target.TargetEntity = target;
        _target.StoppingDistance = GetAttackRange();
    }

    public void SetBehaviourMode(BehaviourMode targetBehaviourMode)
    {
        _behaviourMode = targetBehaviourMode;
    }

    public override void OnActorDamageReceiveDamage(Entity damageSource)
    {
        if (_target == null && _behaviourMode == BehaviourMode.Passive)
        {
            SetAttackTarget(damageSource);
        }
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
            navAgent.stoppingDistance = _target.StoppingDistance;

            while (navAgent != null && (!navAgent.hasPath || navAgent.remainingDistance > _target.StoppingDistance))
            {
                if (!IsTargetValid())
                {
                    break;
                }

                navAgent.SetDestination(_target.GetDestination());

                yield return null;
            }

            if (_target.TargetEntity != null)
            {
                if (Vector3.Distance(_target.GetDestination(), Actor.Position) < _target.StoppingDistance)
                {
                    _target.TargetEntity.DealDamage(Actor.Info.AttackStrength, Actor);

                    var delay = (float) Actor.Info.AttackSpeed;
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
        var potentialTargets = 
            world.EntityMapping.GetEntitiesByRelationship(Actor.FactionId, RelationshipMap.RelationshipType.Hostile);

        var detectionRange = GetDetectionRange();

        foreach (var each in potentialTargets)
        {
            if (Vector3.Distance(each.Position, Actor.Position) <= detectionRange)
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

    private float GetAttackRange()
    {
        return Actor.Info.AttackRange;
    }

    private float GetDetectionRange()
    {
        switch (_behaviourMode)
        {
            case BehaviourMode.Aggressive:
                return Actor.Info.AggressiveDetectionRange;

            case BehaviourMode.Passive:
                return Actor.Info.PassiveDetectionRange;

            case BehaviourMode.Defensive:
                return Actor.Info.DefensiveDetectionRange;
        }

        return 0f;
    }
}