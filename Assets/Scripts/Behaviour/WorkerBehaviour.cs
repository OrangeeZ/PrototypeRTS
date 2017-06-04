using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class WorkerBehaviour : ActorBehaviour
    {
		private Workplace.Workplace _workplace;

		public WorkerBehaviour(Workplace.Workplace workplace) : base()
		{
			_workplace = workplace;
		}

        protected override IEnumerator UpdateRoutine()
        {
            while (true)
            {
                var navAgent = Actor.NavAgent;

                if (!_workplace.HasResources)
                {
                    // navAgent.SetDestination(Actor.BaseWorld.GetClosestStockpileWithResource(_workspace.ResourceType));
                    // while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
                    // {
                    //     yield return null;
                    // }

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
                stockpileBlock.ChangeResource(_workplace.Info.OutputResource, 
                    _workplace.Info.ProductionDuration);

            }
        }
    }
}
