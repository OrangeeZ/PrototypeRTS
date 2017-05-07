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
            Debug.Log("Started working behaviour");

            while (true)
            {
                var navAgent = Actor.NavAgent;

                if (!_workplace.HasResources)
                {
                    // navAgent.SetDestination(Actor.World.GetClosestStockpileWithResource(_workspace.ResourceType));
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
                    duration -= _deltaTime;

                    yield return null;
                }

                _workplace.EndProduction();

                navAgent.SetDestination(Actor.World.GetClosestStockpile(Actor.Position).transform.position);
                while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
                {
                    yield return null;
                }

                Actor.World.GetClosestStockpile(Actor.Position).AddResource(_workplace.ResourceType, _workplace.ProductionRate);
            }
        }
    }
}
