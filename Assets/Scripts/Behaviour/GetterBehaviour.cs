using System.Collections;
using System.Linq;
using Actors;
using Buildings;
using UnityEngine;

namespace Behaviour
{
    public class GetterBehaviour : ActorBehaviour
    {
        private readonly Workplace _workplace;
        private Entity _target;

        public GetterBehaviour(Workplace workplace)
        {
            _workplace = workplace;
        }

        protected IEnumerable GoToWorkplace()
        {
            var navAgent = Actor.NavAgent;

            navAgent.SetDestination(_workplace.Position);
            while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
            {
                yield return null;
            }
        }

        protected IEnumerable FetchResources()
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

        protected IEnumerable RunProductionCycle()
        {
            var duration = _workplace.BeginProduction();
            while (duration > 0f)
            {
                duration -= DeltaTime;

                yield return null;
            }

            _workplace.EndProduction();
        }

        protected IEnumerable CarryProducedResourcesToStockpile()
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
            while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
            {
                yield return null;
            }

            _workplace.PutResourcesToStockpile(closestStockpileBlock);
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

                FetchResources().GetEnumerator();
                while (routine.MoveNext())
                {
                    yield return null;
                }

                RunProductionCycle().GetEnumerator();
                while (routine.MoveNext())
                {
                    yield return null;
                }

                CarryProducedResourcesToStockpile().GetEnumerator();
                while (routine.MoveNext())
                {
                    yield return null;
                }
            }
        }

        private bool IsTargetValid()
        {
            return _target?.Health > 0;
        }

        private void FindAttackTarget()
        {
            var world = Actor.World;
            var entities = world.Entities;
            var targetEntitiesInfo = _workplace.Info.ProductionCycles.InputResource.AssociatedUnitInfo;
            var potentialTargets = entities
                .GetItems()
                .OfType<Actor>()
                .Where(_ => _.Info == targetEntitiesInfo);

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