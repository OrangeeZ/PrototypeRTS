using System.Collections;
using System.Linq;
using Actors;
using Buildings;
using UnityEngine;

namespace Behaviour
{
    public class WorkerBehaviour : ActorBehaviour
    {
        private readonly Workplace _workplace;
        private Entity _target;

        public WorkerBehaviour(Workplace workplace)
        {
            _workplace = workplace;
        }

        protected override IEnumerator UpdateRoutine()
        {
            while (true)
            {
                var routine = GoToWorkplace().GetEnumerator();
                while (routine.MoveNext())
                {
                    yield return null;
                }

                routine = FetchResources().GetEnumerator();
                while (routine.MoveNext())
                {
                    yield return null;
                }

                routine = GoToWorkplace().GetEnumerator();
                while (routine.MoveNext())
                {
                    yield return null;
                }

                routine = RunProductionCycle().GetEnumerator();
                while (routine.MoveNext())
                {
                    yield return null;
                }

                routine = CarryProducedResourcesToStockpile().GetEnumerator();
                while (routine.MoveNext())
                {
                    yield return null;
                }

                yield return null;
            }
        }

        private IEnumerable GoToWorkplace()
        {
            var navAgent = Actor.NavAgent;

            navAgent.SetDestination(_workplace.Position);

            while (navAgent.pathPending)
            {
                yield return null;
            }

            while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
            {
                yield return null;
            }
        }

        private IEnumerable FetchResources()
        {
            var associatedUnitInfo = _workplace.ActiveProductionCycle.InputResource.AssociatedUnitInfo;
            return associatedUnitInfo == null ? FetchResourcesFromStockpile() : FetchResourcesFromUnit();
        }

        private IEnumerable FetchResourcesFromStockpile()
        {
            var navAgent = Actor.NavAgent;

            if (!_workplace.HasResources && _workplace.ActiveProductionCycle.InputResourceQuantity > 0)
            {
                var closestStockpileBlock = default(StockpileBlock);

                do
                {
                    var delay = 1f;
                    while (delay > 0)
                    {
                        delay -= DeltaTime;
                        yield return null;
                    }

                    closestStockpileBlock = _workplace.World.Stockpile.GetClosestStockpileWithResource
                    (
                        _workplace.Position,
                        _workplace.ActiveProductionCycle.InputResource
                    );
                } while (closestStockpileBlock == null);

                navAgent.SetDestination(closestStockpileBlock.Position);
                while (navAgent.pathPending)
                {
                    yield return null;
                }

                while (navAgent.remainingDistance > 1f)
                {
                    yield return null;
                }
            }
        }

        private IEnumerable FetchResourcesFromUnit()
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
                navAgent.SetDestination(_target.Position);
                navAgent.stoppingDistance = Actor.Info.AttackRange;

                while (!navAgent.hasPath)
                {
                    if (!IsTargetValid())
                    {
                        break;
                    }

                    navAgent.SetDestination(_target.Position);

                    yield return null;
                }

                if (_target != null)
                {
                    if (Vector3.Distance(_target.Position, Actor.Position) < Actor.Info.AttackRange)
                    {
                        _target.DealDamage(Actor.Info.AttackStrength, Actor);

                        if (_target.Health <= 0)
                        {
                            yield break;
                        }

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

        private IEnumerable RunProductionCycle()
        {
            var duration = _workplace.BeginProduction();
            while (duration > 0f)
            {
                duration -= DeltaTime;

                yield return null;
            }

            _workplace.EndProduction();
        }

        private IEnumerable CarryProducedResourcesToStockpile()
        {
            var navAgent = Actor.NavAgent;

            var world = Actor.World;
            var closestStockpileBlock = default(StockpileBlock);

            while (true)
            {
                closestStockpileBlock =
                    world.Stockpile.GetClosestStockpileBlock(Actor.Position,
                        _workplace.ActiveProductionCycle.OutputResource);
                if (closestStockpileBlock != null)
                    break;

                yield return null;
            }

            navAgent.SetDestination(closestStockpileBlock.Position);

            while (navAgent.pathPending)
            {
                yield return null;
            }

            while (navAgent.remainingDistance > 1f)
            {
                yield return null;
            }

            _workplace.PutResourcesToStockpile(closestStockpileBlock);
        }

        private bool IsTargetValid()
        {
            return _target?.Health > 0;
        }

        private void FindAttackTarget()
        {
            var world = Actor.World;
            var targetEntitiesInfo = _workplace.ActiveProductionCycle.InputResource.AssociatedUnitInfo;
            var potentialTargets = world.EntityMapping.GetActorsByInfo(targetEntitiesInfo);

            var detectionRange = Actor.Info.AggressiveDetectionRange;

            foreach (var each in potentialTargets)
            {
                if (Vector3.Distance(each.Position, Actor.Position) <= detectionRange)
                {
                    _target = each;

                    return;
                }
            }
        }
    }
}