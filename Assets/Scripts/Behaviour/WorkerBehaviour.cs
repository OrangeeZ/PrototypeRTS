﻿using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class WorkerBehaviour : ActorBehaviour
    {
        private readonly Workplace.Workplace _workplace;

        public WorkerBehaviour(Workplace.Workplace workplace)
        {
            _workplace = workplace;
        }

        protected override IEnumerator UpdateRoutine()
        {
            while (true)
            {
                var navAgent = Actor.NavAgent;

                navAgent.SetDestination(_workplace.Position);
                while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
                {
                    yield return null;
                }
                
                if (!_workplace.HasResources && _workplace.Info.InputResourceQuantity > 0)
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
                            _workplace.Info.InputResource
                        );
                    } while (closestStockpileBlock == null);

                    navAgent.SetDestination(closestStockpileBlock.Position);
                    while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
                    {
                        yield return null;
                    }

                    navAgent.SetDestination(_workplace.Position);
                    while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
                    {
                        yield return null;
                    }
                }

                var duration = _workplace.BeginProduction();
                while (duration > 0f)
                {
                    duration -= DeltaTime;

                    yield return null;
                }

                _workplace.EndProduction();
                var world = Actor.World;
                var stockpileBlock = world.Stockpile.GetClosestStockpileBlock(Actor.Position);
                navAgent.SetDestination(stockpileBlock.Position);
                while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
                {
                    yield return null;
                }

                _workplace.PutResourcesToStockpile(stockpileBlock);
            }
        }
    }
}