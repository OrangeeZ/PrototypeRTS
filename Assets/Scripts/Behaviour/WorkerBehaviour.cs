using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBehaviour : ActorBehaviour
{
    protected override IEnumerator UpdateRoutine()
    {
        Debug.Log("Started working behaviour");

        while (true)
        {
            var navAgent = Actor.NavAgent;

            if (!Actor.Workplace.HasResources)
            {
                // navAgent.SetDestination(Actor.World.GetClosestStockpileWithResource(Actor.Workplace.ResourceType));
                // while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
                // {
                //     yield return null;
                // }

                navAgent.SetDestination(Actor.Workplace.Position);
                while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
                {
                    yield return null;
                }
            }

            var duration = Actor.Workplace.BeginProduction();
            while (duration > 0f)
            {
                duration -= DeltaTime;

                yield return null;
            }

            Actor.Workplace.EndProduction();

            navAgent.SetDestination(Actor.World.GetClosestStockpile(Actor.Position).transform.position);
            while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
            {
                yield return null;
            }

            Actor.World.GetClosestStockpile(Actor.Position).AddResource(Actor.Workplace.ResourceType, Actor.Workplace.ProductionRate);
        }
    }
}
